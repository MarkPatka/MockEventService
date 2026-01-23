using MediatR;
using UserService.Application.ClubManagement.Common;

namespace UserService.Application.ClubManagement.Command.LeaveClubCommand;

public record LeaveClubCommand(Guid  UserId, Guid ClubId) :  IRequest<LeaveClubResult>;