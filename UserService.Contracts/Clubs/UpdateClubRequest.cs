namespace UserService.Contracts.Clubs;

public record UpdateClubRequest(Guid id, string Name, string Description, Guid OwnerId, bool IsPublic);