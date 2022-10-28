using Diploma.Dto;
using Diploma.Mapping;
using Microsoft.AspNetCore.Mvc;
using Diploma.Repository;

namespace Diploma.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuizRepository _quizRepository;

        public HomeController(ILogger<HomeController> logger, IQuizRepository quizRepository)
        {
            _logger = logger;
            _quizRepository = quizRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult loadTest() {
            return RedirectToAction("Test");
        }

        public async Task<IActionResult> Test() {
            var test = await _quizRepository.GetTestByType(true);
            var testDto = test.ToDto();

            return View(testDto);
        }

        [HttpPost]
        public IActionResult CreateTestResult([FromForm]QuizDto quizDto, int age)
        {
            var simpleTestResult = string.Join("", quizDto.QuestionDto
                .SelectMany(x => x.Answers)
                .Where(x => x.IsSelected)
                .Select(x => x.AnswerTextResult));

            //todo check for null

            return RedirectToAction("Modal");
        }

        public IActionResult Modal()
        {
            return View();
        }
    }
}