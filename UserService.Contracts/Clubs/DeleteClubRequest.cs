using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Contracts.Clubs;

public record DeleteClubRequest(Guid id);