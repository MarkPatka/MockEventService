using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.Services;

public interface IClubService
{
    Task<IEnumerable<Club>> SearchClubsAsync(string name, IEnumerable<string> interests = null,
        OwnerId? ownerId = null);

    Task<Club?> GetClubByIdAsync(ClubId clubId);
    Task<IEnumerable<ClubMember>> GetClubMembersAsync(ClubId clubId);

    Task<Club> AddAsync(Club club);
}