using MediatR;
using UserService.Application.ClubManagement.Common;

namespace UserService.Application.ClubManagement.Command.JoinClubCommand;

public record JoinToClubCommand(Guid  UserId, Guid ClubId) :  IRequest<JoinToClubResult>;