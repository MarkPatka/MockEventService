using FluentValidation;

namespace UserService.Application.ClubManagement.Command.CreateClubCommand;

public class CreateClubCommandValidator : AbstractValidator<CreateClubCommand>
{
    public CreateClubCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.OwnerId).NotEmpty();
    }
}