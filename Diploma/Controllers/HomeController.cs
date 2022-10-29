using Diploma.Dto;
using Diploma.Mapping;
using Microsoft.AspNetCore.Mvc;
using Diploma.Repository;
using System.Diagnostics;

namespace Diploma.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuizRepository _quizRepository;
        private readonly IPersonalityRepository _personalityRepository;
        private readonly IUserRepository _userRepository;

        private static bool TestType;
        private static Dictionary<int, TimeSpan> ModalTimeDictionary = new Dictionary<int, TimeSpan>();
        private static int Counter;
        private static string WordTestResult = null!;
        private static Stopwatch? timer;

        public HomeController(ILogger<HomeController> logger, IQuizRepository quizRepository, IPersonalityRepository personalityRepository, IUserRepository userRepository) {
            Counter = 1;
            _logger = logger;
            _quizRepository = quizRepository;
            _personalityRepository = personalityRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index() {
            var rnd = new Random();
            var res = rnd.Next(2);
            TestType = res != 0;
            ViewBag.TestType = TestType;
            return View();
        }

        public async Task<IActionResult> Test() {
            var test = await _quizRepository.GetTestByType(TestType);
            return View(test.ToDto());
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

            //todo check for null

            return RedirectToAction("FirstTask");
        }

        public IActionResult Modal() {
            return View();
        }

        public void SaveModalResult() {
            if (timer == null) {
                throw new Exception();
            }

            timer.Stop();
            ModalTimeDictionary[Counter++] = timer.Elapsed;

            //var foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
        }

        public IActionResult FirstTask() {
            timer = new Stopwatch();
            timer.Start();
            return View();
        }

        public IActionResult FirstTaskSaveResult() {
            return RedirectToAction("SecondTask");
        }

        public IActionResult SecondTask() {
            timer = new Stopwatch();
            timer.Start();
            return View();
        }

        public IActionResult SecondTaskSaveResult() {
            return RedirectToAction("ThirdTask");
        }

        public IActionResult ThirdTask() {
            timer = new Stopwatch();
            timer.Start();
            return View();
        }

        public IActionResult ThirdTaskSaveResult() {
            return RedirectToAction("TestResult");
        }

        public async Task<IActionResult> TestResult() {
            var personality = await _personalityRepository.GetPersonalityByTitle(WordTestResult);
            //var user = new User();
            //await _userRepository.Add(user);
            return View(personality?.ToDto());
        }
    }
}