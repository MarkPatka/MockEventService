using MediatR;
using UserService.Application.ClubManagement.Common;

namespace UserService.Application.ClubManagement.Command.CreateClubCommand;

public record CreateClubCommand(string Name, string Description, Guid OwnerId, bool IsPublic) :  IRequest<CreateClubResult>;