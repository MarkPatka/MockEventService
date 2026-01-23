using MediatR;
using UserService.Application.Persistence;
using UserService.Application.UserProfileManagement.Common;
using UserService.Domain.UserProfileAggregate;

namespace UserService.Application.UserProfileManagement.Queries.GetUserProfileInterestsQuery;

public class GetUserProfileQueryInterestsHandler
    : IRequestHandler<GetUserProfileInterestsQuery, GetUserProfileInterestsResult>
{
    private readonly IUserProfileRepository _repository;

    public GetUserProfileQueryInterestsHandler(IUserProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetUserProfileInterestsResult> Handle(GetUserProfileInterestsQuery query,
        CancellationToken cancellationToken)
    {
        IEnumerable<UserProfile> userProfiles = await _repository
            .GetByFilterAsync(x => x.Id.Value == query.userId)
            .ConfigureAwait(false);

        UserProfile? userProfile = userProfiles.FirstOrDefault();
        if (userProfile == null)
        {
            throw new Exception("UserProfile doesn't exist");
        }

        return await Task.FromResult(new GetUserProfileInterestsResult(userProfile.Interests));
    }
}