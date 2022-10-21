using Diploma.Models;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Repository;

public class QuizRepository : IQuizRepository
{
    private readonly DiplomDbContext _dbContext;

    public QuizRepository(DiplomDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Test> GetTestByType(bool type)
    {
        return await _dbContext.Tests
            .Include(x => x.TestQuestions)
            .ThenInclude(x => x.Question)
            .ThenInclude(x => x.QuestionAnswers)
            .ThenInclude(x => x.Answer)
            .FirstOrDefaultAsync(x => x.TestType == type) ?? throw new InvalidOperationException();
    }
}