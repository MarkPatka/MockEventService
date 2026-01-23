using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Queries.GetClubMembersQuery;

public record GetClubMembersQuery(ClubId id) : IRequest<IEnumerable<GetClubMembersResult>>;