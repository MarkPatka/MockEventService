using MediatR;
using UserService.Application.ClubManagement.Common;

namespace UserService.Application.ClubManagement.Queries.GetClubQuery;

public record GetClubQuery(string id) : IRequest<GetClubResult>;