using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Command.UpdateClubCommand;

public record UpdateClubCommand(
    Guid Id,
    string Name,
    string Description,
    IEnumerable<string> Ineterests,
    Guid OwnerId,
    bool IsPublic)
    : IRequest<UpdateClubResult>;