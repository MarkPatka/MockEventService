using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Contracts.UserProfiles;

public record UpdateUserProfileInterestsRequest(UserId id, IEnumerable<string> interests);