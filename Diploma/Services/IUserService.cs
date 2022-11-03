using Diploma.Models;

namespace Diploma.Services;

public interface IUserService
{
    public Task SaveUserResultInDb(User user, Dictionary<int, (TimeSpan, bool, bool?)> modalTestResults);
}