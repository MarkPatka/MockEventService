using MediatR;
using UserService.Application.Persistence;
using UserService.Application.UserProfileManagement.Common;
using UserService.Domain.UserProfileAggregate;

namespace UserService.Application.UserProfileManagement.Command.UpdateUserProfileCommand;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UpdateUserProfileResult>
{
    private readonly IRepository<UserProfile> _userProfileRepository;

    public UpdateUserProfileCommandHandler(IRepository<UserProfile> userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }

    public async Task<UpdateUserProfileResult> Handle(UpdateUserProfileCommand request,
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
            request.DisplayName,
            request.Bio,
            request.AvatarUri,
            request.Interests,
            request.BirthDate,
            userProfile.CreatedAt,
            DateTime.Now
        );

        userProfile = await _userProfileRepository.UpdateAsync(userProfile).ConfigureAwait(false);

        return new UpdateUserProfileResult(
            userProfile.DisplayName,
            userProfile.Bio,
            userProfile.AvatarUri,
            userProfile.Interests,
            userProfile.BirthDate);
    }
}