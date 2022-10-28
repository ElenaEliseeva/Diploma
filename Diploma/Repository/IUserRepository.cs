using Diploma.Models;

namespace Diploma.Repository;

public interface IUserRepository
{
    public Task Add(User user);
}