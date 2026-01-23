namespace UserService.Contracts.Clubs;

public record UpdateClubResponse(Guid id, string Name, string Description, Guid OwnerId, bool IsPublic);