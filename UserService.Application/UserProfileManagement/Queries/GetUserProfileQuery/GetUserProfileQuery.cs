using MediatR;
using UserService.Application.UserProfileManagement.Common;

namespace UserService.Application.UserProfileManagement.Queries.GetUserProfileQuery;

public record GetUserProfileQuery(Guid userId) : IRequest<GetUserProfileResult>;