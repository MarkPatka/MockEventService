namespace UserService.Contracts.UserProfiles;

public record UpdateUserProfileInterestsRequest(Guid Id, IEnumerable<string> Interests);