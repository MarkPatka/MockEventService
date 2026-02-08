using MediatR;
using UserService.Application.Services;
using UserService.Application.UserProfileManagement.Common;
using UserService.Domain.UserProfileAggregate;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.UserProfileManagement.Command.UpdateUserProfileInterestsCommand;

public class
    UpdateUserProfileInterestsCommandHandler : IRequestHandler<UpdateUserProfileInterestsCommand,
    UpdateUserProfileInterestsResult>
{
    private readonly IUserProfileService _userProfileService;

    public UpdateUserProfileInterestsCommandHandler(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    public async Task<UpdateUserProfileInterestsResult> Handle(UpdateUserProfileInterestsCommand request,
        CancellationToken cancellationToken)
    {
        UserProfile? userProfile =
            await _userProfileService.GetUserProfileByIdAsync(UserId.Create(request.UserId)).ConfigureAwait(false);

        if (userProfile == null)
        {
            throw new Exception("UserProfile doesn't exist");
        }

        userProfile = UserProfile.Update(
            userProfile,
            userProfile.DisplayName,
            userProfile.Bio,
            userProfile.AvatarUri,
            request.Interests,
            userProfile.BirthDate,
            DateTime.UtcNow
        );

        await _userProfileService.UpdateAsync(userProfile).ConfigureAwait(false);

        return new UpdateUserProfileInterestsResult(userProfile.Interests);
    }
}