using MediatR;
using UserService.Application.UserProfileManagement.Common;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.UserProfileManagement.Command.UpdateUserProfileInterestsCommand;

public record UpdateUserProfileInterestsCommand(
    UserId Id,
    IReadOnlyList<string> Interests)
    : IRequest<UpdateUserProfileInterestsResult>;