using AdessoTurkey.Application.Interfaces.Repositories;
using AdessoTurkey.Domain.Entities;
using AdessoTurkey.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AdessoTurkey.Persistence.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Team>> GetAllAsync()
        {
            return await _context.Teams
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Team>> GetByCountryAsync(string country)
        {
            return await _context.Teams
                .AsNoTracking()
                .Where(t => t.Country == country)
                .ToListAsync();
        }
    }
}
