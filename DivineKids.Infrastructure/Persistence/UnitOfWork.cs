using DivineKids.Application.Contracts;

namespace DivineKids.Infrastructure.Persistence;

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork, IDisposable, IAsyncDisposable
{
    private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();

    public ValueTask DisposeAsync() => _context.DisposeAsync();
}