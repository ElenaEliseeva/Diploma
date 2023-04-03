using Diploma.Dto;
using Diploma.Mapping;
using Microsoft.AspNetCore.Mvc;
using Diploma.Repository;
using System.Diagnostics;
using Diploma.Models;
using Diploma.Services;
using Diploma.Helpers;

namespace Diploma.Controllers;

public class TestController : Controller {
    private readonly ILogger<TestController> _logger;
    private readonly IQuizRepository _quizRepository;
    private readonly IPersonalityRepository _personalityRepository;
    private readonly IModalTypeRepository _modalTypeRepository;
    private readonly IUserService _userService;
    private readonly IDateTimeProvider _dateTimeProvider;

    private const string UserAge = "_UserAge";
    private const string ModalType = "_ModalType";
    private const string CurrentTest = "_CurrentTest";
    private const string ClarifyingQuestionOne = "_ClarifyingQuestionOne";
    private const string ClarifyingQuestionTwo = "_ClarifyingQuestionTwo";
    private const string ClarifyingQuestionThree = "_ClarifyingQuestionThree";
    private const string WordTestResult = "_WordTestResult";
    private const string ModalTestResult = "_ModalTestResult";

    private static Dictionary<string, Stopwatch> stopWatchTestTimeDictionary = new Dictionary<string, Stopwatch>();
    private static Dictionary<string, Stopwatch> stopWatchModalTimeDictionary = new Dictionary<string, Stopwatch>();
    private static Dictionary<string, Stopwatch> stopWatchTaskTimeDictionary = new Dictionary<string, Stopwatch>();


    public TestController(ILogger<TestController> logger, IQuizRepository quizRepository,
        IPersonalityRepository personalityRepository, IUserService userService, IModalTypeRepository modalTypeRepository, IDateTimeProvider dateTimeProvider) {
        _logger = logger;
        _quizRepository = quizRepository;
        _personalityRepository = personalityRepository;
        _userService = userService;
        _modalTypeRepository = modalTypeRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public IActionResult Index() {
        return View();
    }

    public async Task<IActionResult> Create() {
        try
        {
            HttpContext.Session.Set<Test>(CurrentTest, await _quizRepository.GetTestByType(false));

            var rnd = new Random();
            var res = rnd.Next(1, 4);

            HttpContext.Session.Set<ModalType>(ModalType, await _modalTypeRepository.GetModalTypeById(res));

            var testTimeTimeStopwatch = new Stopwatch();
            testTimeTimeStopwatch.Start();

            stopWatchTestTimeDictionary[HttpContext.Session.Id] = testTimeTimeStopwatch;


            Response.Cookies.Append("testtype", "2");
            Response.Cookies.Append("picttype", HttpContext.Session.Get<ModalType>(ModalType).ModalTypeId.ToString());
        }
        catch (Exception ex)
        {
            LogWriter.Write("Error in create; " + ex.Message + "; " + ex.StackTrace);
        }

        return View(HttpContext.Session.Get<Test>(CurrentTest).ToDto());
    }

    [HttpPost]
    public IActionResult CreateTestResult([FromForm] QuizDto quizDto)
    {
        var timer = stopWatchTestTimeDictionary[HttpContext.Session.Id];
        timer.Stop();

        try
        {
            HttpContext.Session.Set<string>(WordTestResult, TestResultHelper.CreateWordFromTestResults(quizDto
                .QuestionDto
                .SelectMany(x => x.Answers)
                .Where(x => x.IsSelected)
                .Select(x => char.Parse(x.AnswerTextResult))
                .ToList()));
        }
        catch (Exception ex)
        {
            LogWriter.Write("Error in selecting questions; " + ex.Message + "; " + ex.StackTrace);
        }

        return RedirectToAction("FirstTask");
    }

    public void SaveModalResult(int modalNumber, bool modalResult)
    {
        var modalTimer = stopWatchModalTimeDictionary[HttpContext.Session.Id];
        modalTimer.Stop();

        HttpContext.Session.Set<(TimeSpan modalTime, TimeSpan? testTime, bool modalResult, bool? testResult)>
            ($"{ModalTestResult}{modalNumber}", (modalTimer.Elapsed, null, modalResult, null));

        var taskTimer = new Stopwatch();
        taskTimer.Start();

        stopWatchTaskTimeDictionary[HttpContext.Session.Id] = taskTimer;
    }

    public IActionResult FirstTask() {
        ViewBag.ModalTypeId = HttpContext.Session.Get<ModalType>(ModalType).ModalTypeId;
        return View();
    }

    public void SaveTaskResult(int testNumber, bool selectedAction)
    {
        var taskTimer = stopWatchTaskTimeDictionary[HttpContext.Session.Id];
        taskTimer.Stop();

        var val = HttpContext.Session.Get<(TimeSpan modalTime, TimeSpan? testTime, bool modalResult, bool? testResult)>
            ($"{ModalTestResult}{testNumber}");

        val.testResult = selectedAction;
        val.testTime = taskTimer.Elapsed;

        HttpContext.Session.Set($"{ModalTestResult}{testNumber}", val);
    }

    public void StartTimer() {
        var modalTimer = new Stopwatch();
        modalTimer.Start();

        stopWatchModalTimeDictionary[HttpContext.Session.Id] = modalTimer;
    }

    public IActionResult FirstTaskSaveResult() {
        return RedirectToAction("SecondTask");
    }

    public IActionResult SecondTask() {
        ViewBag.ModalTypeId = HttpContext.Session.Get<ModalType>(ModalType).ModalTypeId;

        return View();
    }

    public IActionResult SecondTaskSaveResult() {
        return RedirectToAction("ThirdTask");
    }

    public IActionResult ThirdTask() {
        ViewBag.ModalTypeId = HttpContext.Session.Get<ModalType>(ModalType).ModalTypeId;

        return View();
    }

    public IActionResult ThirdTaskSaveResult() {
        return RedirectToAction("ClarifyingQuestions");
    }

    public IActionResult ClarifyingQuestions() {
        return View();
    }

    public IActionResult ClarifyingQuestionsSaveResult(int age, string clarifyingQuestionOne, string clarifyingQuestionTwo, string clarifyingQuestionThree) {
        HttpContext.Session.Set<int>(UserAge, age);
        HttpContext.Session.Set<string>(ClarifyingQuestionOne, clarifyingQuestionOne);
        HttpContext.Session.Set<string>(ClarifyingQuestionTwo, clarifyingQuestionTwo);
        HttpContext.Session.Set<string>(ClarifyingQuestionThree, clarifyingQuestionThree);
        return RedirectToAction("TestResult");
    }

    public async Task<IActionResult> TestResult() {

        try
        {
            var wordTestResult = HttpContext.Session.Get<string>(WordTestResult);

            if (string.IsNullOrEmpty(wordTestResult))
            {
                LogWriter.Write("Redirect to Index");
                return RedirectToAction("Index");
            }

            var personality = await _personalityRepository.GetPersonalityByTitle(wordTestResult);

            var user = new User
            {
                Age = HttpContext.Session.Get<int>(UserAge),
                PersonalityId = personality.PersonalityId,
                TestId = 2,
                ModalTypeId = HttpContext.Session.Get<ModalType>(ModalType)!.ModalTypeId,
                UserCreateDate = _dateTimeProvider.DateTimeNow,
                TestTimeResult = stopWatchTestTimeDictionary[HttpContext.Session.Id].Elapsed,
                ClarifyingQuestionOne = HttpContext.Session.Get<string>(ClarifyingQuestionOne)!,
                ClarifyingQuestionTwo = HttpContext.Session.Get<string>(ClarifyingQuestionTwo)!,
                ClarifyingQuestionThree = HttpContext.Session.Get<string>(ClarifyingQuestionThree)
            };

            var dict =
                new Dictionary<int, (TimeSpan modalTime, TimeSpan? testTime, bool modalResult, bool? testResult)>();
            for (int i = 1; i <= 3; i++)
            {
                dict[i] = HttpContext.Session
                    .Get<(TimeSpan modalTime, TimeSpan? testTime, bool modalResult, bool? testResult)>
                        ($"{ModalTestResult}{i}");
            }

            await _userService.SaveUserResultInDb(user, dict);

            // Очищать сессию
            HttpContext.Session.Clear();

            return View(personality.ToDto());
        }
        catch (Exception ex)
        {
            LogWriter.Write("Error in creating test results; " + ex.Message + "; " + ex.StackTrace);
        }

        throw new ArgumentException();
    }
}