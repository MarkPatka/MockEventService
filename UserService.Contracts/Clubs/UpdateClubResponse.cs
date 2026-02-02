namespace UserService.Contracts.Clubs;

public record UpdateClubResponse(
    Guid Id,
    string Name,
    string Description,
    IEnumerable<string> Interests,
    Guid OwnerId,
    bool IsPublic);