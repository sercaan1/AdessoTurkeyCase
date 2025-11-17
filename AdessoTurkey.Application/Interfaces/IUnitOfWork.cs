using AdessoTurkey.Application.Interfaces.Repositories;

namespace AdessoTurkey.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDrawRepository DrawRepository { get; }
        ITeamRepository TeamRepository { get; }  // ⭐ YENİ
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
