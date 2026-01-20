using MediatR;
using UserService.Application.Persistence;
using UserService.Application.UserProfileManagement.Common;
using UserService.Domain.UserProfileAggregate;

namespace UserService.Application.UserProfileManagement.Queries.GetUserProfileQuery;

public class GetUserProfileQueryHandler
    : IRequestHandler<GetUserProfileQuery, GetUserProfileResult>
{
    private readonly IUserProfileRepository _repository;

    public GetUserProfileQueryHandler(IUserProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetUserProfileResult> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<UserProfile> userProfiles = await _repository
            .GetByFilterAsync(x => x.Id.Value.ToString() == request.userId)
            .ConfigureAwait(false);

        var userProfile = userProfiles.ToList().FirstOrDefault();
        if (userProfile != null)
        {
            throw new Exception("UserProfile doesn't exist");
        }

        return await Task.FromResult(
            new GetUserProfileResult(
                userProfile.DisplayName,
                userProfile.Bio,
                userProfile.AvatarUri,
                userProfile.Interests,
                userProfile.BirthDate
            ));
    }
}