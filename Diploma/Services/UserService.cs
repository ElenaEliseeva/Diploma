using Diploma.DataAccess;
using Diploma.Models;

namespace Diploma.Services;

public class UserService : IUserService
{
    private readonly DiplomDbContext _dbContext;
    private readonly ILogger<UserService> _logger;

    public UserService(DiplomDbContext dbContext, ILogger<UserService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SaveUserResultInDb(User user, Dictionary<int, (TimeSpan, TimeSpan?, bool, bool?)> modalTestResults)
    {
        try
        {
            await _dbContext.Database.BeginTransactionAsync();

            var counter = 1;
            foreach (var modalValue in modalTestResults)
            {
                var testResultEntity = new TestResult
                {
                    TestNumber = counter,
                    ModalTimeResult = modalValue.Value.Item1,
                    TestTimeResult = (TimeSpan)modalValue.Value.Item2,
                    ModalResult = modalValue.Value.Item3,
                    TestResultt = (bool)modalValue.Value.Item4,
                    UserId = user.UserId
                };

                counter++;
                user.TestResults.Add(testResultEntity);
                await _dbContext.AddAsync(testResultEntity);
            }

            await _dbContext.AddAsync(user);

            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Error}", ex.Message);
        }
    }
}