using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain;

namespace SportBids.Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey> where TEntity : Entity<TKey>
{
    protected readonly AppDbContext _context;

    public RepositoryBase(AppDbContext context)
    {
        _context = context;
    }

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
    }

    public void Update(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            _context.Set<TEntity>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            Update(entity);
        }
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().FindAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetByIdAsync(ICollection<TKey> ids, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().Where(entity => ids.Contains(entity.Id)).ToListAsync(cancellationToken);
    }

    // protected IQueryable GetAllWhere(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    // {
    //     return _context.Set<TEntity>().Where(predicate);
    // }
}
