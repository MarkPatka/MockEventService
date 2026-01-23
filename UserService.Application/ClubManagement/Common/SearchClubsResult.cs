namespace UserService.Application.ClubManagement.Common;

public record SearchClubsResult(Guid id, string Name, string Description, Guid OwnerId);