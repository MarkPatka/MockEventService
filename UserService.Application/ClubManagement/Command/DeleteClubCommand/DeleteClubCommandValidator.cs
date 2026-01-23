using FluentValidation;

namespace UserService.Application.ClubManagement.Command.DeleteClubCommand;

public class DeleteClubCommandValidator : AbstractValidator<DeleteClubCommand>
{
    public DeleteClubCommandValidator()
    {
        RuleFor(x => x.id).NotEmpty();
    }
}