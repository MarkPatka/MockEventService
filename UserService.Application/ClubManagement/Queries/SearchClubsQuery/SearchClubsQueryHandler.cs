using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Persistence;
using UserService.Application.Services;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Queries.SearchClubsQuery;

public class SearchClubsQueryHandler
    : IRequestHandler<SearchClubsQuery, IEnumerable<SearchClubsResult>>
{
    private readonly IClubService _clubService;

    public SearchClubsQueryHandler(IClubService clubService)
    {
        _clubService = clubService;
    }

    public async Task<IEnumerable<SearchClubsResult>> Handle(SearchClubsQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<Club> clubs =
            await _clubService.SearchClubsAsync(request.Name, request.Interests).ConfigureAwait(false);

        return await Task.FromResult(clubs.Select(c =>
            new SearchClubsResult(c.Id.Value, c.Name, c.Description, c.Interests, c.Owner.Value)));
    }
}