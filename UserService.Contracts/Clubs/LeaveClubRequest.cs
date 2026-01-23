namespace UserService.Contracts.Clubs;

public record LeaveClubRequest(Guid UserId, Guid ClubId);