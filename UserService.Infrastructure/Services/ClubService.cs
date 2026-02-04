using Microsoft.EntityFrameworkCore;
using UserService.Application.ClubManagement.Dto;
using UserService.Application.Persistence;
using UserService.Application.Persistence.Specifications.Clubs;
using UserService.Application.Services;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Infrastructure.Services;

public class ClubService : IClubService
{
    private readonly IRepository<Club, ClubId> _repository;
    private readonly IUserClubQueryRepository  _userClubQueryRepository;

    public ClubService(IRepository<Club, ClubId> repository,
        IUserClubQueryRepository  userClubQueryRepository)
    {
        _repository = repository;
        _userClubQueryRepository = userClubQueryRepository;
    }

    public async Task<IEnumerable<Club>> SearchClubsAsync(string name, IEnumerable<string>? interests = null,
        OwnerId? ownerId = null)
    {
        var spec = new SearchClubsSpec(name, interests, ownerId);
        return await _repository.ListAsync(spec).ConfigureAwait(false);
    }

    public async Task<Club?> GetClubByIdAsync(ClubId clubId)
    {
        var spec = new ClubByIdSpec(clubId);
        return await _repository.FirstOrDefaultAsync(spec).ConfigureAwait(false);
    }

    public async Task<IEnumerable<ClubDto>> GetClubsByUserAsync(UserId userId)
    {
        return await _userClubQueryRepository.GetClubsByUserAsync(userId);
    }

    //какая - то хрень
    /*public async Task<IEnumerable<Club>> GetClubsByUserAsync(UserId userId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await context.Clubs
            .Where(c => EF.Property<List<ClubMember>>(c, "_members")
                .Any(m => m.UserId == userId))
            .ToListAsync();
    }*/

    public async Task<IEnumerable<ClubMember>> GetClubMembersAsync(ClubId clubId)
    {
        var club = await GetClubByIdAsync(clubId).ConfigureAwait(false);
        return club != null ? club.ClubMembers : Enumerable.Empty<ClubMember>();
    }

    public async Task<bool> IsMemberParticipatedAsync(ClubId clubId, UserId userId)
    {
        var club = await GetClubByIdAsync(clubId).ConfigureAwait(false);
        return club != null && club.ClubMembers.Any(x => x.UserId == userId);
    }

    public async Task<Club> AddAsync(Club club)
    {
        return await _repository.AddAsync(club).ConfigureAwait(false);
    }

    public async Task AddMember(Club club, UserId userId, DateTime joinTime)
    {
        club.AddMember(userId, joinTime);
        await _repository.UpdateAsync(club).ConfigureAwait(false);
    }

    public async Task DeleteMember(Club club, UserId userId, DateTime joinTime)
    {
        club.DeleteMember(userId, joinTime);
        await _repository.UpdateAsync(club).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Club club)
    {
        await _repository.DeleteAsync(club).ConfigureAwait(false);
    }

    public async Task UpdateAsync(Club club)
    {
        await _repository.UpdateAsync(club).ConfigureAwait(false);
    }
}