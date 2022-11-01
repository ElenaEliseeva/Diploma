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

    public async Task SaveUserResultInDb(User user, List<(TimeSpan, bool)> modalValues)
    {
        try
        {
            await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.AddAsync(user);

            var counter = 1;
            foreach (var modalValue in modalValues)
            {
                var modalTimeEntity = new ModalTime
                {
                    ModalNumber = counter++,
                    ModalTimeResult = modalValue.Item1,
                    ModalResult = modalValue.Item2,
                    UserId = user.UserId
                };
                user.ModalTimes.Add(modalTimeEntity);
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