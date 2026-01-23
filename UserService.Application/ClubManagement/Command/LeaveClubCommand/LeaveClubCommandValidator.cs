using FluentValidation;
using UserService.Application.ClubManagement.Command.JoinClubCommand;

namespace UserService.Application.ClubManagement.Command.LeaveClubCommand;

public class LeaveClubCommandValidator : AbstractValidator<JoinToClubCommand>
{
    public LeaveClubCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ClubId).NotEmpty();
    }
}