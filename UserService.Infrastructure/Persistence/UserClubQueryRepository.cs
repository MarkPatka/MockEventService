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

    public async Task<IReadOnlyList<ClubDto>> GetClubsByUser(UserId userId)
    {
        await using var context = _dbContextFactory.CreateDbContext();

        return await (
            from club in context.Clubs.AsNoTracking()
            join member in context.Set<ClubMember>()
                on club.Id equals member.ClubId
            where member.UserId == userId
            select new ClubDto
            {
                Id = club.Id.Value,
                Name = club.Name,
                Description = club.Description,
                IsPublic = club.IsPublic,
                CreatedAt = club.CreatedAt
            }
        ).ToListAsync();
    }
}