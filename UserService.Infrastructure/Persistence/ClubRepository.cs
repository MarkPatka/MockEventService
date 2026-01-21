using UserService.Application.Persistence;
using System.Linq.Expressions;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.UserProfileAggregate;

namespace UserService.Infrastructure.Persistence;

public class ClubRepository : IClubRepository
{
    public ClubRepository()
    {
    }

    public Task AddAsync(Club entity)
    {
        return Task.CompletedTask;
    }

    public Task<Club> GetAsync(Expression<Func<Club, bool>> expression)
    {
        return Task.FromResult(Club.Create("1", "1", OwnerId.CreateUnique(), true,
            DateTime.Now, null));
    }

    public Task<IEnumerable<Club>> GetByFilterAsync(Expression<Func<Club, bool>> expression)
    {
        return Task.FromResult(new List<Club>()
        {
            Club.Create("1", "1", OwnerId.CreateUnique(), true,
                DateTime.Now, null)
        }.AsEnumerable());
    }

    public Task<Club> UpdateAsync(Club entity)
    {
        return Task.FromResult(entity);
    }

    public Task DeleteAsync(Club entity)
    {
        return Task.CompletedTask;
    }
}