using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.ClubManagement.Dto;
using UserService.Application.Persistence;
using UserService.Application.Services;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Queries.GetClubsByUserQuery;

public class GetClubsQueryByUserHandler
    : IRequestHandler<GetClubsByUserQuery, IEnumerable<GetClubsByUserResult>>
{
    private readonly IClubService _clubService;

    public GetClubsQueryByUserHandler(IClubService clubService)
    {
        _clubService = clubService;
    }

    public async Task<IEnumerable<GetClubsByUserResult>> Handle(GetClubsByUserQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<ClubDto> clubs = await _clubService.GetClubsByUserAsync(UserId.Create(request.UserId))
            .ConfigureAwait(false);

        return await Task.FromResult(clubs.Select(c =>
            new GetClubsByUserResult(c.Id, c.Name, c.Description, c.Owner, c.IsPublic)));
    }
}