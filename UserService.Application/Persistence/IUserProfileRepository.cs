using UserService.Domain.UserProfileAggregate;

namespace UserService.Application.Persistence;

public interface IUserProfileRepository : IRepository<UserProfile>;
