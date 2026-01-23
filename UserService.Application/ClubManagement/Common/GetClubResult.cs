namespace UserService.Application.ClubManagement.Common;

public record GetClubResult(Guid id, string Name, string Description, Guid OwnerId, bool IsPublic);