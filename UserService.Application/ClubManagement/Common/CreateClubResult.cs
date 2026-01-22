using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Common;

public record CreateClubResult(ClubId id, string Name, string Description, OwnerId OwnerId, bool IsPublic);