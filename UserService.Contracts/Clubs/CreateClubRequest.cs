namespace UserService.Contracts.Clubs;

public record CreateClubRequest(
    string Name,
    string Description,
    IEnumerable<string> Interests,
    Guid OwnerId,
    bool IsPublic);