namespace UserService.Contracts.Clubs;

public record UpdateClubRequest(
    Guid Id,
    string Name,
    string Description,
    IEnumerable<string> Interests,
    Guid OwnerId,
    bool IsPublic);