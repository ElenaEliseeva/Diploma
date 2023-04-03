using Diploma.DataAccess;
using Diploma.Helpers;
using Diploma.Models;

namespace Diploma.Services;

public class UserService : IUserService
{
    private readonly DiplomDbContext _dbContext;

    public UserService(DiplomDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveUserResultInDb(User user, Dictionary<int, (TimeSpan, TimeSpan?, bool, bool?)> modalTestResults)
    {
        try
        {
            var counter = 1;
            foreach (var modalValue in modalTestResults)
            {
                var testResultEntity = new TestResult
                {
                    TestNumber = counter,
                    ModalTimeResult = modalValue.Value.Item1,
                    TestTimeResult = modalValue.Value.Item2,
                    ModalResult = modalValue.Value.Item3,
                    TestResultt = modalValue.Value.Item4,
                    UserId = user.UserId
                };

                counter++;
                user.TestResults.Add(testResultEntity);
                await _dbContext.AddAsync(testResultEntity);
            }

            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            LogWriter.Write("Success write! User age: " + user.Age);
        }
        catch (Exception ex)
        {
            LogWriter.Write(ex.Message + "; " + ex.StackTrace);
        }
    }
}