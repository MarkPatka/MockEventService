using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Contracts.UserProfiles;

public record GetUserProfileRequest(UserId UserId);