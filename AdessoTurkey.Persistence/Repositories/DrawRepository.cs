using AdessoTurkey.Application.Interfaces.Repositories;
using AdessoTurkey.Domain.Entities;
using AdessoTurkey.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AdessoTurkey.Persistence.Repositories
{
    public class DrawRepository : IDrawRepository
    {
        private readonly ApplicationDbContext _context;

        public DrawRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Draw?> GetByIdAsync(int id)
        {
            return await _context.Draws
                .Include(d => d.Groups)
                    .ThenInclude(g => g.Teams)
                        .ThenInclude(dt => dt.Team)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<Draw>> GetAllAsync()
        {
            return await _context.Draws
                .Include(d => d.Groups)
                    .ThenInclude(g => g.Teams)
                        .ThenInclude(dt => dt.Team)
                .AsNoTracking()
                .OrderByDescending(d => d.DrawDate)
                .ToListAsync();
        }

        public async Task AddAsync(Draw draw)
        {
            await _context.Draws.AddAsync(draw);
        }

        public async Task UpdateAsync(Draw draw)
        {
            _context.Draws.Update(draw);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var draw = await _context.Draws.FindAsync(id);
            if (draw != null)
            {
                _context.Draws.Remove(draw);
            }
        }
    }
}