using Diploma.Dto;
using Diploma.Mapping;
using Microsoft.AspNetCore.Mvc;
using Diploma.Repository;
using System.Diagnostics;
using Diploma.Models;
using Diploma.Services;

namespace Diploma.Controllers;

public class HomeController : Controller
{
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

    public IActionResult Index()
    {
        var rnd = new Random();
        var res = rnd.Next(2);
        TestType = res != 0;
        ViewBag.TestType = TestType;
        return View();
    }

    public async Task<IActionResult> Test()
    {
        TestType = false;
        CurrentTest = await _quizRepository.GetTestByType(TestType);
        return View(CurrentTest.ToDto());
    }

    [HttpPost]
    public IActionResult CreateTestResult([FromForm] QuizDto quizDto, int age)
    {
        switch (TestType)
        {
            case true:
                WordTestResult = string.Join("", quizDto.QuestionDto
                    .SelectMany(x => x.Answers)
                    .Where(x => x.IsSelected)
                    .Select(x => x.AnswerTextResult));
                break;
            case false:
                WordTestResult = SolveTest(quizDto.QuestionDto
                    .SelectMany(x => x.Answers)
                    .Where(x => x.IsSelected)
                    .Select(x => char.Parse(x.AnswerTextResult))
                    .ToList());
                break;
        }

        UserAge = age;

        return RedirectToAction("FirstTask");
    }

    private string SolveTest(IReadOnlyList<char> testResult)
    {
        var dict = new Dictionary<char, int>
        {
            { 'E', 0 },
            { 'I', 0 },
            { 'S', 0 },
            { 'N', 0 },
            { 'T', 0 },
            { 'F', 0 },
            { 'J', 0 },
            { 'P', 0 }
        };

        for (int i = 0; i < testResult.Count; i++)
        {
            switch (i)
            {
                case 0 or 7 or 14 or 28 when testResult[i] == 'A':
                    dict['E']++;
                    break;
                case 0 or 7 or 14 or 28:
                    dict['I']++;
                    break;
                case 1 or 8 or 15 or 22 or 29 or 2 or 9 or 16 or 23 or 30 when testResult[i] == 'A':
                    dict['S']++;
                    break;
                case 1 or 8 or 15 or 22 or 29 or 2 or 9 or 16 or 23 or 30:
                    dict['N']++;
                    break;
                case 3 or 10 or 17 or 24 or 31 or 4 or 11 or 18 or 25 or 32 when testResult[i] == 'A':
                    dict['T']++;
                    break;
                case 3 or 10 or 17 or 24 or 31 or 4 or 11 or 18 or 25 or 32:
                    dict['F']++;
                    break;
                case 5 or 12 or 19 or 26 or 33 or 6 or 13 or 20 or 27 or 34 when testResult[i] == 'A':
                    dict['J']++;
                    break;
                case 5 or 12 or 19 or 26 or 33 or 6 or 13 or 20 or 27 or 34:
                    dict['P']++;
                    break;
            }
        }

        return string.Join("", new List<char>
        {
            dict.FirstOrDefault(x => x.Value == Math.Max(dict['E'], dict['I']) && x.Key is 'E' or 'I').Key,
            dict.FirstOrDefault(x => x.Value == Math.Max(dict['S'], dict['N']) && x.Key is 'S' or 'N').Key,
            dict.FirstOrDefault(x => x.Value == Math.Max(dict['T'], dict['F']) && x.Key is 'T' or 'F').Key,
            dict.FirstOrDefault(x => x.Value == Math.Max(dict['J'], dict['P']) && x.Key is 'J' or 'P').Key
        });
    }

    public void SaveModalResult(int modalNumber)
    {
        if (timer == null)
        {
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

    public async Task<IActionResult> FirstTask()
    {
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

    public IActionResult SecondTask()
    {
        timer = new Stopwatch();
        timer.Start();

        ViewBag.ModalTypeId = ModalType.ModalTypeId;

        return View();
    }

    public IActionResult SecondTaskSaveResult()
    {
        return RedirectToAction("ThirdTask");
    }

    public IActionResult ThirdTask()
    {
        timer = new Stopwatch();
        timer.Start();

        ViewBag.ModalTypeId = ModalType.ModalTypeId;

        return View();
    }

    public IActionResult ThirdTaskSaveResult()
    {
        return RedirectToAction("TestResult");
    }

    public async Task<IActionResult> TestResult()
    {
        var personality = await _personalityRepository.GetPersonalityByTitle(WordTestResult);

        if (personality == null)
        {
            throw new Exception("Personality wasn't found!");
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