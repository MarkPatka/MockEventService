using System.Collections;
using UserService.Application.Common.Exceptions;
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
    public ClubService(IRepository<Club, ClubId> repository) => _repository = repository;

    public async Task<IEnumerable<Club>> SearchClubsAsync(string name, IEnumerable<string>? interests = null,
        OwnerId? ownerId = null)
    {
        var spec = new SearchClubsSpec(name, interests, ownerId);
        return await _repository.ListAsync(spec).ConfigureAwait(false);
    }

    public async Task<Club?> GetClubByIdAsync(ClubId clubId)
    {
        var spec = new ClubByIdSpec(clubId);
        IEnumerable<Club> clubs = await _repository.ListAsync(spec).ConfigureAwait(false);
        return clubs.FirstOrDefault();
    }

    public async Task<IEnumerable<Club>> GetClubsByIdsAsync(IEnumerable<ClubId> clubIds)
    {
        var spec = new ClubsByIdsSpec(clubIds);
        return await _repository.ListAsync(spec).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Club>> GetClubsByUserAsync(UserId userId)
    {
        var spec = new ClubByUserSpec(userId);
        return await _repository.ListAsync(spec).ConfigureAwait(false);
    }

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

    public async Task AddMember(ClubId clubId, UserId userId)
    {
        var club = await GetClubByIdAsync(clubId).ConfigureAwait(false);
        if (club == null)
        {
            throw new EntityNotFoundException("Club not found");
        }

        club.AddMember(userId, DateTime.Now);
        await _repository.UpdateAsync(club).ConfigureAwait(false);
    }

    public async Task DeleteMember(ClubId clubId, UserId userId)
    {
        var club = await GetClubByIdAsync(clubId).ConfigureAwait(false);
        if (club == null)
        {
            throw new EntityNotFoundException("Club not found");
        }

        club.DeleteMember(userId, DateTime.Now);
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