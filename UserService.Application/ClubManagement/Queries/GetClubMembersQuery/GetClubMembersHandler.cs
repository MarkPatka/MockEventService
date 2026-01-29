using MapsterMapper;
using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Persistence;
using UserService.Application.Services;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Queries.GetClubMembersQuery;

public class GetClubMembersHandler
    : IRequestHandler<GetClubMembersQuery, IEnumerable<GetClubMembersResult>>
{
    private readonly IClubService _clubService;

    public GetClubMembersHandler(IClubService clubService)
    {
        _clubService = clubService;
    }

    public async Task<IEnumerable<GetClubMembersResult>> Handle(GetClubMembersQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<ClubMember> clubMembers = await _clubService.GetClubMembersAsync(request.id)
            .ConfigureAwait(false);

        return await Task.FromResult(clubMembers.Select(c => new GetClubMembersResult(c.UserId)));
    }
}