using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Command.UpdateClubCommand;

public record UpdateClubCommand(ClubId id, string Name, string Description, Guid OwnerId, bool IsPublic)
    : IRequest<UpdateClubResult>;