using FluentValidation;

namespace MockEventService.Application.EventManagement.Command.CreateEventCommand;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
    }
}
