using Diploma.Models;

namespace Diploma.Repository;

public class UserRepository : IUserRepository
{
    private readonly DiplomDbContext _dbContext;
    
    public UserRepository(DiplomDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(User user)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
}