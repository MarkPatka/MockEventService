using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.Persistence.Specifications.Clubs;

public class ClubByUserSpec : BaseSpecification<Club>
{
    public ClubByUserSpec(UserId userId) : base(c => c.ClubMembers.Select(m => m.UserId).Contains(userId))
    {
    }
}