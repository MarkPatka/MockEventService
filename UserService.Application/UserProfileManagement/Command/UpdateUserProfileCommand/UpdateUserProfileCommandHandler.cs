using MediatR;
using UserService.Application.Persistence;
using UserService.Application.Services;
using UserService.Application.UserProfileManagement.Common;
using UserService.Domain.UserProfileAggregate;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.UserProfileManagement.Command.UpdateUserProfileCommand;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UpdateUserProfileResult>
{
    private readonly IUserProfileService _userProfileService;

    public UpdateUserProfileCommandHandler(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    public async Task<UpdateUserProfileResult> Handle(UpdateUserProfileCommand request,
        CancellationToken cancellationToken)
    {
        UserProfile? userProfile = await _userProfileService.GetUserProfileByIdAsync(UserId.Create(request.Id))
            .ConfigureAwait(false);

        if (userProfile == null)
        {
            throw new Exception("UserProfile doesn't exist");
        }

        userProfile = UserProfile.Create(
            userProfile.Id,
            request.DisplayName,
            request.Bio,
            request.AvatarUri,
            request.Interests,
            request.BirthDate,
            userProfile.CreatedAt,
            DateTime.Now
        );

        await _userProfileService.UpdateAsync(userProfile).ConfigureAwait(false);

        return new UpdateUserProfileResult(
            userProfile.DisplayName,
            userProfile.Bio,
            userProfile.AvatarUri,
            userProfile.Interests,
            userProfile.BirthDate);
    }
}