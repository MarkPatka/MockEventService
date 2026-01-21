using MediatR;
using UserService.Application.Persistence;
using UserService.Application.UserProfileManagement.Common;
using UserService.Domain.UserProfileAggregate;

namespace UserService.Application.UserProfileManagement.Command.UpdateUserProfileInterestsCommand;

public class
    UpdateUserProfileInterestsCommandHandler : IRequestHandler<UpdateUserProfileInterestsCommand,
    UpdateUserProfileInterestsResult>
{
    private readonly IUserProfileRepository _userProfileRepository;

    public UpdateUserProfileInterestsCommandHandler(IUserProfileRepository userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }

    public async Task<UpdateUserProfileInterestsResult> Handle(UpdateUserProfileInterestsCommand request,
        CancellationToken cancellationToken)
    {
        IEnumerable<UserProfile> userProfiles =
            await _userProfileRepository.GetByFilterAsync(x => x.Id == request.Id).ConfigureAwait(false);

        var userProfile = userProfiles.ToList().FirstOrDefault();
        if (userProfile != null)
        {
            throw new Exception("UserProfile doesn't exist");
        }

        userProfile = UserProfile.Create(
            userProfile.Id,
            userProfile.DisplayName,
            userProfile.Bio,
            userProfile.AvatarUri,
            request.Interests,
            userProfile.BirthDate,
            userProfile.CreatedAt,
            DateTime.Now
        );

        userProfile = await _userProfileRepository.UpdateAsync(userProfile).ConfigureAwait(false);

        return new UpdateUserProfileInterestsResult(userProfile.Interests);
    }
}