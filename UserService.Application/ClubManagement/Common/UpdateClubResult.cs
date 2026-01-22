using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Common;

public record UpdateClubResult(ClubId id, string Name, string Description, OwnerId OwnerId, bool IsPublic);