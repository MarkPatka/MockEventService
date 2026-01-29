using UserService.Application.Persistence;
using UserService.Application.Persistence.Specifications;
using UserService.Application.Services;
using UserService.Domain.UserProfileAggregate;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Infrastructure.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IRepository<UserProfile, UserId> _repository;

    public UserProfileService(IRepository<UserProfile, UserId> repository)
    {
        _repository = repository;
    }

    public async Task<UserProfile> InsertAsync(UserProfile userProfile, CancellationToken cancellationToken = default)
    {
        return await _repository.AddAsync(userProfile, cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(UserProfile userProfile, CancellationToken cancellationToken = default)
    {
        await _repository.UpdateAsync(userProfile, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<string?>> GetUsersInterestsAsync(UserId userId,
        CancellationToken cancellationToken = default)
    {
        var spec = new UsersInterestsByUserIdSpec(userId);
        IReadOnlyList<UserProfile> userProfiles =
            await _repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        return userProfiles.FirstOrDefault()?.Interests ?? new List<string>();
    }

    public async Task<UserProfile?> GetUserProfileAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var spec = new UsersInterestsByUserIdSpec(userId);
        IReadOnlyList<UserProfile> userProfiles =
            await _repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        return userProfiles.FirstOrDefault();
    }

    public async Task DeleteAsync(UserProfile userProfile, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(userProfile, cancellationToken).ConfigureAwait(false);
    }
}