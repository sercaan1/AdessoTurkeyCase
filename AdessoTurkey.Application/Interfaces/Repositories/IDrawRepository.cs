using AdessoTurkey.Domain.Entities;

namespace AdessoTurkey.Application.Interfaces.Repositories
{
    public interface IDrawRepository
    {
        Task<Draw?> GetByIdAsync(int id);
        Task<List<Draw>> GetAllAsync();
        Task AddAsync(Draw draw);
        Task UpdateAsync(Draw draw);
        Task DeleteAsync(int id);
    }
}
