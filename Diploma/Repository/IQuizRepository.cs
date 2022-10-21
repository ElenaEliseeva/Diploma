using Diploma.Models;

namespace Diploma.Repository;

public interface IQuizRepository
{
    public Task<Test> GetTestByType(bool type);
}