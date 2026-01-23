using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Command.CreateClubCommand;

public record CreateClubCommand(string Name, string Description, OwnerId OwnerId, bool IsPublic) :  IRequest<CreateClubResult>;