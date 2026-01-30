using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.Persistence.Specifications.Clubs;

public class ClubByIdSpec : BaseSpecification<Club>
{
    public ClubByIdSpec(ClubId clubId) : base(c => c.Id == clubId)
    {
    }
}