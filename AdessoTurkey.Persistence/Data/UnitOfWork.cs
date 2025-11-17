using AdessoTurkey.Application.Interfaces;
using AdessoTurkey.Application.Interfaces.Repositories;
using AdessoTurkey.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace AdessoTurkey.Persistence.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        private IDrawRepository? _drawRepository;
        private ITeamRepository? _teamRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IDrawRepository DrawRepository
        {
            get
            {
                _drawRepository ??= new DrawRepository(_context);
                return _drawRepository;
            }
        }

        public ITeamRepository TeamRepository
        {
            get
            {
                _teamRepository ??= new TeamRepository(_context);
                return _teamRepository;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
