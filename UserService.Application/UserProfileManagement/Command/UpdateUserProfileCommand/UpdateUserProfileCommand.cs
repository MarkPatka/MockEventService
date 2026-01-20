using MediatR;
using UserService.Application.UserProfileManagement.Common;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.UserProfileManagement.Command.UpdateUserProfileCommand;

public record UpdateUserProfileCommand(
    UserId Id,
    string DisplayName,
    string Bio,
    Uri? AvatarUri,
    IReadOnlyList<string> Interests,
    DateTime? BirthDate) 
    : IRequest<UpdateUserProfileResult>;