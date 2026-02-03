namespace UserService.Application.ClubManagement.Common;

public record JoinToClubResult(Guid ClubId, Guid UserId, DateTime JoinedAt);