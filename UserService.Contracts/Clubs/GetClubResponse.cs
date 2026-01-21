namespace UserService.Contracts.Clubs;

public record GetClubResponse(string id, string Name, string Description, string OwnerId, bool IsPublic);