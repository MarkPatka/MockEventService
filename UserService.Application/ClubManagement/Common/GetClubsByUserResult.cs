namespace UserService.Application.ClubManagement.Common;

public record GetClubsByUserResult(Guid id, string Name, string Description, Guid OwnerId, bool IsPublic);