using MediatR;
using UserService.Application.UserProfileManagement.Common;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.UserProfileManagement.Queries.GetUserProfileInterestsQuery;

public record GetUserProfileInterestsQuery(UserId userId) : IRequest<GetUserProfileInterestsResult>;