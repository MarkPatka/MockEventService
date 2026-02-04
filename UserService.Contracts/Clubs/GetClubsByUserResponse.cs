namespace UserService.Contracts.Clubs;

public record GetClubsByUserResponse(Guid Id, string Name, string Description, Guid OwnerId, bool IsPublic);