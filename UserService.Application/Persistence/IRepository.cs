using System.Linq.Expressions;

namespace UserService.Application.Persistence;

public interface IRepository<TEntity>
{
    public Task AddAsync(TEntity entity);
    public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);
    public Task<IEnumerable<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> expression);
    public Task<TEntity> UpdateAsync(TEntity entity);
    public Task DeleteAsync(TEntity entity);
}
