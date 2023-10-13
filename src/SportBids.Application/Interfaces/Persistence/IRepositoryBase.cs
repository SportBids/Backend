using SportBids.Domain;

namespace SportBids.Application.Interfaces.Persistence;

public interface IRepositoryBase<TEntity, TKey>
{
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);

    Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken);
}
