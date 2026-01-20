namespace UserService.Contracts.UserProfiles;

public record UpdateUserProfileRequest(
    string DisplayName,
    string Bio,
    Uri? AvatarUri,
    IReadOnlyList<string> Interests,
    DateTime? BirthDate);