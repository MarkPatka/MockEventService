using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.Persistence.Specifications.Clubs;

public class ClubsByIdsSpec : BaseSpecification<Club>
{
    public ClubsByIdsSpec(IEnumerable<ClubId> clubIds) : base(c => clubIds.Contains(c.Id))
    {
    }
}