namespace UserService.Contracts.Clubs;

public record GetClubResponse(string id, string Name, string Description, Guid OwnerId, bool IsPublic);