using Diploma.Dto;
using Diploma.Mapping;
using Microsoft.AspNetCore.Mvc;
using Diploma.Repository;
using System.Diagnostics;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Services;

namespace Diploma.Controllers;

public class TestController : Controller
{
    private readonly ILogger<TestController> _logger;
    private readonly IQuizRepository _quizRepository;
    private readonly IPersonalityRepository _personalityRepository;
    private readonly IModalTypeRepository _modalTypeRepository;
    private readonly IUserService _userService;

    private static bool TestType;
    private static int UserAge;
    private static ModalType? ModalType;
    private static Test? CurrentTest;

    private static readonly Dictionary<int, (TimeSpan modalTime, bool modalResult, bool? testResult)> resDictionary = new();

    private static string WordTestResult = null!;
    private static Stopwatch? timer;

    public TestController(ILogger<TestController> logger, IQuizRepository quizRepository,
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

    public async Task<IActionResult> Create()
    {
        CurrentTest = await _quizRepository.GetTestByType(TestType);

        var rnd = new Random();
        var res = rnd.Next(1, 4);
        ModalType = await _modalTypeRepository.GetModalTypeById(res);

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
                WordTestResult = TestResultHelper.CreateWordFromTestResults(quizDto.QuestionDto
                    .SelectMany(x => x.Answers)
                    .Where(x => x.IsSelected)
                    .Select(x => char.Parse(x.AnswerTextResult))
                    .ToList());
                break;
        }

        UserAge = age;

        return RedirectToAction("FirstTask");
    }

    public void SaveModalResult(int modalNumber, bool modalResult)
    {
        if (timer == null)
        {
            throw new Exception();
        }
        timer.Stop();

        switch (modalNumber)
        {
            case 1:
                resDictionary[1] = (timer.Elapsed, modalResult, null);
                break;
            case 2:
                resDictionary[2] = (timer.Elapsed, modalResult, null);
                break;
            case 3:
                resDictionary[3] = (timer.Elapsed, modalResult, null);
                break;
        }
    }

    public IActionResult FirstTask()
    {
        timer = new Stopwatch();
        timer.Start();

        ViewBag.ModalTypeId = ModalType.ModalTypeId;

        return View();
    }

    public void SaveTaskResult(int testNumber, bool selectedAction)
    {
        if (!resDictionary.TryGetValue(testNumber, out var val)) return;
        val.testResult = selectedAction;
        resDictionary[testNumber] = val;
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
            _logger.LogInformation("Personality was null {personality}; Word test result: {WordTestResult}", personality, WordTestResult);
            return RedirectToAction("Index");
        }

        var user = new User
        {
            Age = UserAge,
            PersonalityId = personality.PersonalityId,
            TestId = CurrentTest.TestId,
            ModalTypeId = ModalType.ModalTypeId
        };

        await _userService.SaveUserResultInDb(user, resDictionary);

        return View(personality.ToDto());
    }
}