using Diploma.Dto;
using Diploma.Mapping;
using Microsoft.AspNetCore.Mvc;
using Diploma.Repository;
using System.Diagnostics;
using Diploma.Helpers;
using Diploma.Models;
using Diploma.Services;

namespace Diploma.Controllers;

public class TestController : Controller {
    private readonly ILogger<TestController> _logger;
    private readonly IQuizRepository _quizRepository;
    private readonly IPersonalityRepository _personalityRepository;
    private readonly IModalTypeRepository _modalTypeRepository;
    private readonly IUserService _userService;
    private readonly IDateTimeProvider _dateTimeProvider;

    private static bool TestType;
    private static int UserAge;
    private static ModalType? ModalType;
    private static Test? CurrentTest;

    private static Dictionary<int, (TimeSpan modalTime, TimeSpan? testTime, bool modalResult, bool? testResult)>? ModalTestResultDictionary;

    private static string ClarifyingQuestionOne;
    private static string ClarifyingQuestionTwo;
    private static string ClarifyingQuestionThree;

    private static string WordTestResult = null!;
    private static Stopwatch? ModalTimer;
    private static Stopwatch? TaskTimer;
    private static Stopwatch? TestTimeTimer;

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
        var rnd = new Random();
        var res = rnd.Next(2);
        TestType = res != 0;
        ViewBag.TestType = TestType;

        ModalTestResultDictionary = new Dictionary<int, (TimeSpan modalTime, TimeSpan? testTime, bool modalResult, bool? testResult)>();

        return View();
    }

    public async Task<IActionResult> Create() {
        CurrentTest = await _quizRepository.GetTestByType(TestType);
        var rnd = new Random();
        var res = rnd.Next(1, 4);
        ModalType = await _modalTypeRepository.GetModalTypeById(res);
        TestTimeTimer = new Stopwatch();
        TestTimeTimer.Start();

        Response.Cookies.Append("testtype", CurrentTest.TestId.ToString());
        Response.Cookies.Append("picttype", ModalType.ModalTypeId.ToString());

        return View(CurrentTest.ToDto());
    }

    [HttpPost]
    public IActionResult CreateTestResult([FromForm] QuizDto quizDto) {

        TestTimeTimer.Stop();
        switch (TestType) {
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

        return RedirectToAction("FirstTask");
    }

    public void SaveModalResult(int modalNumber, bool modalResult) {
        if (ModalTimer == null) {
            throw new Exception();
        }
        ModalTimer.Stop();

        switch (modalNumber) {
            case 1:
                ModalTestResultDictionary[1] = (ModalTimer.Elapsed, null, modalResult, null);
                break;
            case 2:
                ModalTestResultDictionary[2] = (ModalTimer.Elapsed, null, modalResult, null);
                break;
            case 3:
                ModalTestResultDictionary[3] = (ModalTimer.Elapsed, null, modalResult, null);
                break;
        }

        TaskTimer = new Stopwatch();
        TaskTimer.Start();
    }

    public IActionResult FirstTask() {
        ViewBag.ModalTypeId = ModalType.ModalTypeId;

        return View();
    }

    public void SaveTaskResult(int testNumber, bool selectedAction) {
        TaskTimer.Stop();
        if (!ModalTestResultDictionary.TryGetValue(testNumber, out var val)) return;
        val.testResult = selectedAction;
        val.testTime = TaskTimer.Elapsed;
        ModalTestResultDictionary[testNumber] = val;
    }

    public void StartTimer() {
        ModalTimer = new Stopwatch();
        ModalTimer.Start();
    }

    public IActionResult FirstTaskSaveResult() {
        return RedirectToAction("SecondTask");
    }

    public IActionResult SecondTask() {
        ViewBag.ModalTypeId = ModalType.ModalTypeId;

        return View();
    }

    public IActionResult SecondTaskSaveResult() {
        return RedirectToAction("ThirdTask");
    }

    public IActionResult ThirdTask() {
        ViewBag.ModalTypeId = ModalType.ModalTypeId;

        return View();
    }

    public IActionResult ThirdTaskSaveResult() {
        return RedirectToAction("ClarifyingQuestions");
    }

    public IActionResult ClarifyingQuestions() {
        return View();
    }

    public IActionResult ClarifyingQuestionsSaveResult(int age, string clarifyingQuestionOne, string clarifyingQuestionTwo, string clarifyingQuestionThree) {
        UserAge = age;
        ClarifyingQuestionOne = clarifyingQuestionOne;
        ClarifyingQuestionTwo = clarifyingQuestionTwo;
        ClarifyingQuestionThree = clarifyingQuestionThree;
        return RedirectToAction("TestResult");
    }

    public async Task<IActionResult> TestResult() {
        if (string.IsNullOrEmpty(WordTestResult) || ModalTestResultDictionary == null!) {
            _logger.LogInformation("Word test result: {WordTestResult}; Modal test dictionary: {ModalTestResultDictionary}", WordTestResult, ModalTestResultDictionary);
            return RedirectToAction("Index");
        }

        var personality = await _personalityRepository.GetPersonalityByTitle(WordTestResult);

        var user = new User {
            Age = UserAge,
            PersonalityId = personality.PersonalityId,
            TestId = CurrentTest.TestId,
            ModalTypeId = ModalType.ModalTypeId,
            UserCreateDate = _dateTimeProvider.DateTimeNow,
            TestTimeResult = TestTimeTimer.Elapsed,
            ClarifyingQuestionOne = ClarifyingQuestionOne,
            ClarifyingQuestionTwo = ClarifyingQuestionTwo,
            ClarifyingQuestionThree = ClarifyingQuestionThree
        };

        await _userService.SaveUserResultInDb(user, ModalTestResultDictionary);

        ModalTestResultDictionary = null!;
        WordTestResult = string.Empty;

        return View(personality.ToDto());
    }
}