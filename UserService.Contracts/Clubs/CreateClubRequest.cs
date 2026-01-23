using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Contracts.Clubs;

public record CreateClubRequest(string Name, string Description, Guid OwnerId, bool IsPublic);