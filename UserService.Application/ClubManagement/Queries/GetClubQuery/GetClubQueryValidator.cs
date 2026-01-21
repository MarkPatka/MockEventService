using FluentValidation;

namespace UserService.Application.ClubManagement.Queries.GetClubQuery;

public class GetClubQueryValidator : AbstractValidator<GetClubQuery>
{
    public GetClubQueryValidator()
    {
        RuleFor(x => x.id).NotEmpty();
    }
}