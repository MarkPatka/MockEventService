namespace UserService.Contracts.Clubs;

public record UpdateClubResponse(string id, string Name, string Description, string OwnerId, bool IsPublic);