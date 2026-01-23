using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Persistence;
using UserService.Domain.ClubAggregate;

namespace UserService.Application.ClubManagement.Queries.GetClubQuery;

public class GetClubQueryHandler
    : IRequestHandler<GetClubQuery, GetClubResult>
{
    private readonly IClubRepository _repository;

    public GetClubQueryHandler(IClubRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetClubResult> Handle(GetClubQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Club> clubs = await _repository
            .GetByFilterAsync(x => x.Id.Value.ToString() == request.id.ToString())
            .ConfigureAwait(false);

        var club = clubs.ToList().FirstOrDefault();
        if (club == null)
        {
            throw new EntityNotFoundException("Club doesn't exist");
        }

        return await Task.FromResult(
            new GetClubResult(
                club.Id.Value,
                club.Name,
                club.Description, 
                club.Owner.Value, 
                club.IsPublic)
        );
    }
}