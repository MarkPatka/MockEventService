using UserService.Application.ClubManagement.Dto;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.Persistence;

public interface IUserClubQueryRepository
{
    Task<IReadOnlyList<ClubDto>> GetClubsByUserAsync(UserId userId);
}