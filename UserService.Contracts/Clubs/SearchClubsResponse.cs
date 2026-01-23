namespace UserService.Contracts.Clubs;

public record SearchClubsResponse(Guid id, string Name, string Description, Guid OwnerId);