using MockEventService.Application.Persistence;
using MockEventService.Application.Services;
using MockEventService.Domain;
using System.Linq.Expressions;

namespace MockEventService.Infrastructure.Persistence;

public class EventRepository : IEventRepository<Event>
{
    private readonly ITimeProviderService _timeProviderService;

    public EventRepository(ITimeProviderService timeProviderService)
    {
        _timeProviderService = timeProviderService;
    }

    public Task Add<T>(T entity)
    {
        throw new NotImplementedException();
    }

    public Task Delete<T>(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<T> Get<T>(Expression<Func<T, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetByFilter<T>(Expression<Func<T, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task Update<T>(T entity)
    {
        throw new NotImplementedException();
    }
}
