using MediatR;
using UserService.Application.ClubManagement.Command.JoinClubCommand;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Persistence;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Command.LeaveClubCommand;

public class LeaveClubCommandHandler : IRequestHandler<LeaveClubCommand, LeaveClubResult>
{
    private readonly IClubMembersRepository _repository;

    public LeaveClubCommandHandler(IClubMembersRepository repository)
    {
        _repository = repository;
    }

    public async Task<LeaveClubResult> Handle(LeaveClubCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<ClubMember> clubs = await _repository
            .GetByFilterAsync(x => x.ClubId.Value == request.ClubId && x.UserId.Value == request.UserId)
            .ConfigureAwait(false);

        var clubMembers = clubs.ToList();
        if ((clubMembers?.Count ?? 0) > 0)
        {
            throw new AlreadyDoneException("You have already left the club");
        }
        await _repository.DeleteAsync(clubMembers.First()).ConfigureAwait(false);

        return await Task.FromResult(new LeaveClubResult());
    }
}