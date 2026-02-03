using MediatR;
using UserService.Application.ClubManagement.Common;

namespace UserService.Application.ClubManagement.Command.DeleteClubCommand;

public record DeleteClubCommand(Guid Id) : IRequest<DeleteClubResult>;