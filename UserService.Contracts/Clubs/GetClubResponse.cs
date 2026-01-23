namespace UserService.Contracts.Clubs;

public record GetClubResponse(Guid id, string Name, string Description, Guid OwnerId, bool IsPublic);