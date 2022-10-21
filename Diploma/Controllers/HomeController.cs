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
            var test = await _quizRepository.GetTestByType(true);
            var testDto = test.ToDto();

            return View();
        }
    }
}