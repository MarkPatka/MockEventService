using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Services;
using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Command.LeaveClubCommand;

public class LeaveClubCommandHandler : IRequestHandler<LeaveClubCommand, LeaveClubResult>
{
    private readonly IClubService _clubService;

    public LeaveClubCommandHandler(IClubService clubService)
    {
        _clubService = clubService;
    }

    public async Task<LeaveClubResult> Handle(LeaveClubCommand request, CancellationToken cancellationToken)
    {
        bool isMemberParticipated = await _clubService
            .IsMemberParticipatedAsync(ClubId.Create(request.ClubId), UserId.Create(request.UserId))
            .ConfigureAwait(false);

        if (!isMemberParticipated)
        {
            throw new AlreadyDoneException("You have already left the club");
        }

        await _clubService.DeleteMember(ClubId.Create(request.ClubId), UserId.Create(request.UserId))
            .ConfigureAwait(false);

        return await Task.FromResult(new LeaveClubResult());
    }
}