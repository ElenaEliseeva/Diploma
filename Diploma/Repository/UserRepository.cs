using Diploma.Models;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Repository;

public class UserRepository : IUserRepository
{
    private readonly DiplomDbContext _dbContext;
    
    public UserRepository(DiplomDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserById(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);
        return user ?? null;
    }

    public async Task Add(User user)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
}