namespace UserService.Application.ClubManagement.Common;

public record GetClubMembersResult(Guid UserId, DateTime JoinedAt);