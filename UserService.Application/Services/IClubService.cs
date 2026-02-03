using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.Services;

public interface IClubService
{
    Task<IEnumerable<Club>> SearchClubsAsync(string name, IEnumerable<string>? interests = null,
        OwnerId? ownerId = null);

    Task<Club?> GetClubByIdAsync(ClubId clubId);
    Task<IEnumerable<Club>> GetClubsByIdsAsync(IEnumerable<ClubId> clubIds);
    Task<IEnumerable<Club>> GetClubsByUserAsync(UserId userId);
    Task<IEnumerable<ClubMember>> GetClubMembersAsync(ClubId clubId);
    Task<bool> IsMemberParticipatedAsync(ClubId clubId, UserId userId);

    Task<Club> AddAsync(Club club);

    Task AddMember(Club club, UserId userId, DateTime joinTime);
    Task DeleteMember(Club clubId, UserId userId, DateTime joinTime);
    Task DeleteAsync(Club club);
    Task UpdateAsync(Club club);
}