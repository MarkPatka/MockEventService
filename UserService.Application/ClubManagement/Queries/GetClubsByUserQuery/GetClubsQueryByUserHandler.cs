using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Persistence;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Queries.GetClubsByUserQuery;

public class GetClubsQueryByUserHandler
    : IRequestHandler<GetClubsByUserQuery, IEnumerable<GetClubsByUserResult>>
{
    private readonly IRepository<Club, ClubId> _repository;

    public GetClubsQueryByUserHandler(IRepository<Club, ClubId> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetClubsByUserResult>> Handle(GetClubsByUserQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<ClubMember> clubMembers = await _repository
            .GetByFilterAsync(x => x.UserId == request.UserId)
            .ConfigureAwait(false);

        var enumerable = clubMembers.ToList();
        if (!enumerable.Any())
        {
            return new List<GetClubsByUserResult>();
        }

        IEnumerable<ClubId> clubIds = enumerable.Select(x => x.ClubId);
        IEnumerable<Club> clubs =
            await _clubRepository.GetByFilterAsync(x => clubIds.Contains(x.Id)).ConfigureAwait(false);

        return await Task.FromResult(clubs.Select(c =>
            new GetClubsByUserResult(c.Id.Value, c.Name, c.Description, c.Owner.Value, c.IsPublic)));
    }
}