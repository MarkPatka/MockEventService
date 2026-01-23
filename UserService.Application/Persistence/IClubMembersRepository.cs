using UserService.Domain.ClubAggregate;
using UserService.Domain.UserProfileAggregate;

namespace UserService.Application.Persistence;

public interface IClubMembersRepository : IRepository<ClubMember>;
