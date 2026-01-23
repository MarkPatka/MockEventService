using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Common;

public record UpdateClubResult(Guid id, string Name, string Description, Guid OwnerId, bool IsPublic);