using Microsoft.EntityFrameworkCore;
using UserService.Application.ClubManagement.Dto;
using UserService.Application.Persistence;
using UserService.Domain.ClubAggregate;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Infrastructure.Persistence;

internal sealed class UserClubQueryRepository : IUserClubQueryRepository
{
    protected readonly IDbContextFactory<UserServiceDbContext> _dbContextFactory;

    public UserClubQueryRepository(IDbContextFactory<UserServiceDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IReadOnlyList<ClubDto>> GetClubsByUserAsync(UserId userId)
    {
        await using var context = _dbContextFactory.CreateDbContext();

        return await context.Clubs
            .AsNoTracking()
            .Where(c => EF.Property<List<ClubMember>>(c, "_members")
                .Any(m => m.UserId == userId))
            .Select(c => new ClubDto
            {
                Id = c.Id.Value,
                Name = c.Name,
                Description = c.Description,
                Owner = c.Owner.Value,
                IsPublic = c.IsPublic,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();
    }
}

public sealed class ClubMemberRead
{
    public Guid ClubId { get; init; }
    public Guid UserId { get; init; }
    public DateTime JoinedAt { get; init; }
}