using System.Linq.Expressions;

namespace MockEventService.Application.Persistence;

public interface IRepository<TEntity> 
{
    public Task Add<T>(T entity);
    public Task<T> Get<T>(Expression<Func<T, bool>> expression);
    public Task<IEnumerable<T>> GetByFilter<T>(Expression<Func<T, bool>> expression);
    public Task Update<T>(T entity);
    public Task Delete<T>(T entity);
}
