using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Persistence;
using UserService.Application.Services;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Queries.GetClubQuery;

public class GetClubQueryHandler
    : IRequestHandler<GetClubQuery, GetClubResult>
{
    private readonly IClubService _clubService;

    public GetClubQueryHandler(IClubService clubService)
    {
        _clubService = clubService;
    }

    public async Task<GetClubResult> Handle(GetClubQuery request, CancellationToken cancellationToken)
    {
        Club? club = await _clubService.GetClubByIdAsync(request.id).ConfigureAwait(false);

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