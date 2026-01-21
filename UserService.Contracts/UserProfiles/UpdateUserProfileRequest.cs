namespace UserService.Contracts.UserProfiles;

public record UpdateUserProfileRequest(
    Guid UserId,
    string DisplayName,
    string Bio,
    Uri? AvatarUri,
    IReadOnlyList<string> Interests,
    DateTime? BirthDate);