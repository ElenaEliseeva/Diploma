using Diploma.Models;

namespace Diploma.Services;

public interface IUserService
{
    public Task SaveUserResultInDb(User user, List<TimeSpan> modalTimes);
}