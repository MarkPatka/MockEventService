using UserService.Application.Persistence;
using System.Linq.Expressions;
using UserService.Domain.UserProfileAggregate;

namespace UserService.Infrastructure.Persistence;

public class UserProfileRepository : IUserProfileRepository
{
    public UserProfileRepository()
    {
    }

    public Task<UserProfile> AddAsync(UserProfile entity)
    {
        return Task.FromResult(UserProfile.Create("1", "1", null, new List<string>(), null,
            DateTime.Now, DateTime.Now));
    }

    public Task<UserProfile> GetAsync(Expression<Func<UserProfile, bool>> expression)
    {
        return Task.FromResult(UserProfile.Create("1", "1", null, new List<string>(), null,
            DateTime.Now, null));
    }

    public Task<IEnumerable<UserProfile>> GetByFilterAsync(Expression<Func<UserProfile, bool>> expression)
    {
        return Task.FromResult(new List<UserProfile>()
        {
            UserProfile.Create("1", "1", null, new List<string>(), null,
                DateTime.Now, null)
        }.AsEnumerable());
    }

    public Task<UserProfile> UpdateAsync(UserProfile entity)
    {
        return Task.FromResult(entity);
    }

    public Task DeleteAsync(UserProfile entity)
    {
        return Task.CompletedTask;
    }
}