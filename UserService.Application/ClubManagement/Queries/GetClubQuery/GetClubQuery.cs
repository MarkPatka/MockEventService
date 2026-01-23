using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Queries.GetClubQuery;

public record GetClubQuery(ClubId id) : IRequest<GetClubResult>;