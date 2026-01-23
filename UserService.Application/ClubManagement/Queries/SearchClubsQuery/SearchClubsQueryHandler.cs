using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Persistence;
using UserService.Domain.ClubAggregate;

namespace UserService.Application.ClubManagement.Queries.SearchClubsQuery;

public class SearchClubsQueryHandler
    : IRequestHandler<SearchClubsQuery, IEnumerable<SearchClubsResult>>
{
    private readonly IClubRepository _repository;

    public SearchClubsQueryHandler(IClubRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SearchClubsResult>> Handle(SearchClubsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Club> clubs = await _repository
            .GetByFilterAsync(x => x.Name == request.Name || request.Interests.Contains(x.Name))
            .ConfigureAwait(false);

        return await Task.FromResult(clubs.Select(c =>
            new SearchClubsResult(c.Id.Value, c.Name, c.Description, c.Owner.Value)));
    }
}