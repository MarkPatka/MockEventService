namespace UserService.Application.ClubManagement.Common;

public record GetClubResult(string? id, string Name, string Description, string? OwnerId, bool IsPublic);