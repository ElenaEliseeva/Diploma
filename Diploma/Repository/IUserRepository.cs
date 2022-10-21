using Diploma.Models;

namespace Diploma.Repository;

public interface IUserRepository
{
    public Task<User?> GetUserById(int id);
    public Task Add(User user);
}