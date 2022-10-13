using Diploma.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Diploma.DataAccess;

namespace Diploma.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QuizDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, QuizDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var seedDatabase = new SeedDatabase(_dbContext);
            await seedDatabase.Initialize();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}