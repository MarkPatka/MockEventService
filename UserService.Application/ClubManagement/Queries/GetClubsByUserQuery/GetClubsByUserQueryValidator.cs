using FluentValidation;

namespace UserService.Application.ClubManagement.Queries.GetClubsByUserQuery;

public class GetClubsByUserQueryValidator : AbstractValidator<GetClubsByUserQuery>
{
    public GetClubsByUserQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}