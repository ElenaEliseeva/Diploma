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

    public async Task SaveUserResultInDb(User user, Dictionary<int, (TimeSpan, bool, bool?)> modalTestResults)
    {
        try
        {
            await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.AddAsync(user);

            var counter = 1;
            foreach (var modalValue in modalTestResults)
            {
                var modalTimeEntity = new ModalTime
                {
                    ModalNumber = counter,
                    ModalTimeResult = modalValue.Value.Item1,
                    ModalResult = modalValue.Value.Item2,
                    UserId = user.UserId
                };

                var secondTestEntity = new SecondTest
                {
                    UserId = user.UserId,
                    SecondTestNumber = counter,
                    SecondTestResult = (bool)modalValue.Value.Item3
                };

                counter++;
                user.ModalTimes.Add(modalTimeEntity);
                user.SecondTests.Add(secondTestEntity);
                await _dbContext.AddAsync(modalTimeEntity);
            }

            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Error}", ex.Message);
        }
    }
}