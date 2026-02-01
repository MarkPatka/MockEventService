using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.Persistence.Specifications.Clubs;

public sealed class SearchClubsSpec : BaseSpecification<Club>
{
    public SearchClubsSpec(string name, IEnumerable<string> interests = null, OwnerId? ownerId = null)
    {
        AddCriteria(c => c.Name == name);

        if (ownerId?.Value is not null)
            AddCriteria(c => c.Owner == ownerId);

        if (interests != null && interests.Any())
            AddCriteria(c => c.Interests.Any(i => interests.Contains(i)));
    }
}