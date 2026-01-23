using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Queries.GetClubsByUserQuery;

public record GetClubsByUserQuery(UserId UserId) : IRequest<IEnumerable<GetClubsByUserResult>>;