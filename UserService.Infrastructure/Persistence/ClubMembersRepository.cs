using UserService.Application.Persistence;
using System.Linq.Expressions;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.UserProfileAggregate;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Infrastructure.Persistence;

public class ClubMembersRepository : IClubMembersRepository
{
    public ClubMembersRepository() { }

    public Task<ClubMember> AddAsync(ClubMember entity)
    {
        return Task.FromResult(ClubMember.Create(entity.ClubId, entity.UserId, entity.JoinedAt));
    }

    public Task<ClubMember> GetAsync(Expression<Func<ClubMember, bool>> expression)
    {
        return Task.FromResult(ClubMember.Create(ClubId.CreateUnique(), UserId.CreateUnique(), DateTime.Now));
    }

    public Task<IEnumerable<ClubMember>> GetByFilterAsync(Expression<Func<ClubMember, bool>> expression)
    {
        return Task.FromResult(new List<ClubMember>()
        {
            ClubMember.Create(ClubId.CreateUnique(), UserId.CreateUnique(), DateTime.Now)
        }.AsEnumerable());
    }

    public Task<ClubMember> UpdateAsync(ClubMember entity)
    {
        return Task.FromResult(entity);
    }

    public Task DeleteAsync(ClubMember entity)
    {
        return Task.CompletedTask;
    }
}