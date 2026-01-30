using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.Persistence.Specifications.Clubs;

public class SearchClubsSpec : BaseSpecification<Club>
{
    public SearchClubsSpec(string Name, IEnumerable<string> interests = null, OwnerId? ownerId = null)
        : base(c => c.Name == Name && c.Interests == interests && c.Owner == ownerId)
    {
    }
}