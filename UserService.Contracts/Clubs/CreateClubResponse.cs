namespace UserService.Contracts.Clubs;

public record CreateClubResponse(string id, string Name, string Description, string OwnerId, bool IsPublic);