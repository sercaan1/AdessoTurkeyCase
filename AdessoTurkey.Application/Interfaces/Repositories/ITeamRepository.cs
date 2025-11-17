using AdessoTurkey.Domain.Entities;

namespace AdessoTurkey.Application.Interfaces.Repositories
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetAllAsync();
        Task<List<Team>> GetByCountryAsync(string country);
    }
}
