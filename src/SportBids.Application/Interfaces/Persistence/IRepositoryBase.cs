namespace SportBids.Application.Interfaces.Persistence;

public interface IRepositoryBase<TEntity, TKey>
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetByIdAsync(ICollection<TKey> ids, CancellationToken cancellationToken);

    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);
    void DeleteRange(IEnumerable<TEntity> entities);
}
