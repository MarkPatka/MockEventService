using UserService.Domain.ClubAggregate;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.Persistence.Specifications.Clubs;

public class ClubsByUserSpec : BaseSpecification<Club>
{
    public ClubsByUserSpec(UserId userId) : base(c => c.ClubMembers.Any(c => c.UserId == userId))
    {
    }
}