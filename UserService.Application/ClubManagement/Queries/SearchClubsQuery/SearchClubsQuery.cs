using MediatR;
using UserService.Application.ClubManagement.Common;

namespace UserService.Application.ClubManagement.Queries.SearchClubsQuery;

public record SearchClubsQuery(string Name, IEnumerable<string> Interests) : IRequest<IEnumerable<SearchClubsResult>>;