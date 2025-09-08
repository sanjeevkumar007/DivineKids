using DivineKids.Application.Contracts;
using DivineKids.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DivineKids.Infrastructure.Repositories;

public class GenericRepository<T>(AppDbContext context, IUnitOfWork unitOfWork) : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    private DbSet<T> Set => _context.Set<T>();

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Set.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await Set.FindAsync([id], cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await Set.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        Set.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException($"{typeof(T).Name} with Id={id} was not found.");
        Set.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}