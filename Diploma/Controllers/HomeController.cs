using Diploma.Dto;
using Diploma.Mapping;
using Microsoft.AspNetCore.Mvc;
using Diploma.Repository;
using System.Diagnostics;
using Diploma.Models;
using Diploma.Services;

namespace Diploma.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuizRepository _quizRepository;
        private readonly IPersonalityRepository _personalityRepository;
        private readonly IModalTypeRepository _modalTypeRepository;
        private readonly IUserService _userService;

        private static bool TestType;
        private static int UserAge;
        private static ModalType? ModalType;
        private static Test? CurrentTest;
        private static TimeSpan FirstModalTime;
        private static TimeSpan SecondModalTime;
        private static TimeSpan ThirdModalTime;
        private static string WordTestResult = null!;
        private static Stopwatch? timer;

        public HomeController(ILogger<HomeController> logger, IQuizRepository quizRepository,
            IPersonalityRepository personalityRepository, IUserService userService, IModalTypeRepository modalTypeRepository)
        {
            _logger = logger;
            _quizRepository = quizRepository;
            _personalityRepository = personalityRepository;
            _userService = userService;
            _modalTypeRepository = modalTypeRepository;
        }

        public IActionResult Index() {
            var rnd = new Random();
            var res = rnd.Next(2);
            TestType = res != 0;
            ViewBag.TestType = TestType;
            return View();
        }

        public async Task<IActionResult> Test() {
            CurrentTest = await _quizRepository.GetTestByType(TestType);
            return View(CurrentTest.ToDto());
        }

        [HttpPost]
        public IActionResult CreateTestResult([FromForm] QuizDto quizDto, int age) {
            switch (TestType) {
                case true:
                    WordTestResult = string.Join("", quizDto.QuestionDto
                        .SelectMany(x => x.Answers)
                        .Where(x => x.IsSelected)
                        .Select(x => x.AnswerTextResult));
                    break;
                case false:
                    break;
            }

            UserAge = age;

            return RedirectToAction("FirstTask");
        }

        public void SaveModalResult(int modalNumber) {
            if (timer == null) {
                throw new Exception();
            }
            timer.Stop();

            switch (modalNumber)
            {
                case 1:
                    FirstModalTime = timer.Elapsed;
                    break;
                case 2:
                    SecondModalTime = timer.Elapsed;
                    break;
                case 3:
                    ThirdModalTime = timer.Elapsed;
                    break;
            }
        }

        public async Task<IActionResult> FirstTask() {
            var rnd = new Random();
            var res = rnd.Next(1, 4);
            ModalType = await _modalTypeRepository.GetModalTypeById(res);

            ViewBag.ModalTypeId = ModalType.ModalTypeId;

            timer = new Stopwatch();
            timer.Start();
            return View();
        }

        public IActionResult FirstTaskSaveResult()
        {
            return RedirectToAction("SecondTask");
        }

        public IActionResult SecondTask() {
            timer = new Stopwatch();
            timer.Start();

            ViewBag.ModalTypeId = ModalType.ModalTypeId;

            return View();
        }

        public IActionResult SecondTaskSaveResult() {
            return RedirectToAction("ThirdTask");
        }

        public IActionResult ThirdTask() {
            timer = new Stopwatch();
            timer.Start();

            ViewBag.ModalTypeId = ModalType.ModalTypeId;

            return View();
        }

        public IActionResult ThirdTaskSaveResult() {
            return RedirectToAction("TestResult");
        }

        public async Task<IActionResult> TestResult() {
            var personality = await _personalityRepository.GetPersonalityByTitle(WordTestResult);
            if (personality == null)
            {
                throw new ArgumentNullException();
            }
            var user = new User
            {
                Age = UserAge,
                PersonalityId = personality.PersonalityId,
                TestId = CurrentTest.TestId,
                ModalTypeId = ModalType.ModalTypeId
            };

            await _userService.SaveUserResultInDb(user, new List<TimeSpan>
            {
                FirstModalTime, SecondModalTime, ThirdModalTime
            });

            return View(personality.ToDto());
        }
    }
}