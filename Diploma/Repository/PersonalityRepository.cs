using Diploma.Models;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Repository;

public class PersonalityRepository : IPersonalityRepository
{
    private readonly DiplomDbContext _dbContext;

    public PersonalityRepository(DiplomDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<Personality?> GetPersonalityByTitle(string title)
    {
        var personality = await _dbContext.Personalities.FirstOrDefaultAsync(x => x.PersonalityTitle == title);
        return personality ?? null;
    }
}