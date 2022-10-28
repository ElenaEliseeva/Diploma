using Diploma.Models;

namespace Diploma.Repository;

public interface IPersonalityRepository
{
    public Task<Personality?> GetPersonalityByTitle(string title);
}