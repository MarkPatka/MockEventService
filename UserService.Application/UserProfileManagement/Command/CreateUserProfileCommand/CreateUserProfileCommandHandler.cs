using MediatR;
using UserService.Application.Services;
using UserService.Application.UserProfileManagement.Common;
using UserService.Domain.UserProfileAggregate;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.UserProfileManagement.Command.CreateUserProfileCommand;

public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, CreateUserProfileResult>
{
    private readonly IUserProfileService _userProfileService;

    public CreateUserProfileCommandHandler(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    public async Task<CreateUserProfileResult> Handle(CreateUserProfileCommand request,
        CancellationToken cancellationToken)
    {
        UserProfile? userProfile = await _userProfileService.GetUserByNameAsync(request.DisplayName)
            .ConfigureAwait(false);

        if (userProfile != null)
        {
            throw new Exception("UserProfile already exists");
        }

        userProfile = UserProfile.Create(
            UserId.CreateUnique(),
            request.DisplayName,
            request.Bio,
            request.AvatarUri,
            request.Interests,
            request.BirthDate,
            DateTime.UtcNow,
            DateTime.UtcNow);

        await _userProfileService.InsertAsync(userProfile).ConfigureAwait(false);

        return new CreateUserProfileResult(
            userProfile.Id.Value,
            userProfile.DisplayName,
            userProfile.Bio,
            userProfile.AvatarUri,
            userProfile.Interests,
            userProfile.BirthDate);
    }
}