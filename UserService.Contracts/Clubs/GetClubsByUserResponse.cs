namespace UserService.Contracts.Clubs;

public record GetClubsByUserResponse(Guid id, string Name, string Description, Guid OwnerId, bool IsPublic);