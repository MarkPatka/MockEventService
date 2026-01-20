using UserService.Application.Persistence;
using System.Linq.Expressions;
using UserService.Domain.UserProfileAggregate;

namespace UserService.Infrastructure.Persistence;

public class UserProfileRepository : IUserProfileRepository
{
    public UserProfileRepository()
    {
    }

    public Task AddAsync(UserProfile entity)
    {
        throw new NotImplementedException();
    }

    public Task<UserProfile> GetAsync(Expression<Func<UserProfile, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserProfile>> GetByFilterAsync(Expression<Func<UserProfile, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task<UserProfile> UpdateAsync(UserProfile entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(UserProfile entity)
    {
        throw new NotImplementedException();
    }
}
