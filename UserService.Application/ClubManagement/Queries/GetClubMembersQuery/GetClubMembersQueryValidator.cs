using FluentValidation;

namespace UserService.Application.ClubManagement.Queries.GetClubMembersQuery;

public class GetClubMembersQueryValidator : AbstractValidator<GetClubQuery.GetClubQuery>
{
    public GetClubMembersQueryValidator()
    {
        RuleFor(x => x.id).NotEmpty();
    }
}