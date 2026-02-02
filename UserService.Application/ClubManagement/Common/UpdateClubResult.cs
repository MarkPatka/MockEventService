namespace UserService.Application.ClubManagement.Common;

public record UpdateClubResult(
    Guid Id,
    string Name,
    string Description,
    IEnumerable<string> Interests,
    Guid OwnerId,
    bool IsPublic);