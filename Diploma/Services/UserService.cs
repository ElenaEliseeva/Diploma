using Diploma.DataAccess;
using Diploma.Models;

namespace Diploma.Services;

public class UserService : IUserService
{
    private static int Counter;
    private readonly DiplomDbContext _dbContext;

    public UserService(DiplomDbContext dbContext)
    {
        Counter = 1;
        _dbContext = dbContext;
    }

    public async Task SaveUserResultInDb(User user, List<TimeSpan> modalTimes)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        //foreach (var modalTime in modalTimes)
        //{
        //    var modalTimeEntity = new ModalTime
        //    {
        //        ModalNumber = Counter++,
        //        ModalTime1 = 
        //    }
        //}
    }
}