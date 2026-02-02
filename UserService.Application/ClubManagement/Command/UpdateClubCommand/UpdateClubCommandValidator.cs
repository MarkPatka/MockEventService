using FluentValidation;

namespace UserService.Application.ClubManagement.Command.UpdateClubCommand;

public class UpdateClubCommandValidator : AbstractValidator<UpdateClubCommand>
{
    public UpdateClubCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.OwnerId).NotEmpty();
    }
}