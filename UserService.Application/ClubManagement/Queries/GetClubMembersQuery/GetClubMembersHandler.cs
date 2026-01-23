using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Persistence;
using UserService.Domain.ClubAggregate;

namespace UserService.Application.ClubManagement.Queries.GetClubMembersQuery;

public class GetClubMembersHandler
    : IRequestHandler<GetClubMembersQuery, IEnumerable<GetClubMembersResult>>
{
    private readonly IClubMembersRepository _repository;

    public GetClubMembersHandler(IClubMembersRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetClubMembersResult>> Handle(GetClubMembersQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<ClubMember> clubMembers = await _repository
            .GetByFilterAsync(x => x.ClubId == request.id)
            .ConfigureAwait(false);
        
        return await Task.FromResult(clubMembers.Select(c => new GetClubMembersResult(c.UserId)));
    }
}