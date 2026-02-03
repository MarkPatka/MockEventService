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
        var clubId = ClubId.Create(request.ClubId);
        var userId = UserId.Create(request.UserId);
        var club = await _clubService.GetClubByIdAsync(clubId).ConfigureAwait(false);
        if (club == null)
        {
            throw new EntityNotFoundException("Club not found");
        }

        bool isMemberParticipated = await _clubService
            .IsMemberParticipatedAsync(clubId, userId).ConfigureAwait(false);
        if (!isMemberParticipated)
        {
            throw new AlreadyDoneException("You have already left the club");
        }
        
        await _clubService.DeleteMember(club, userId, DateTime.UtcNow).ConfigureAwait(false);
        return await Task.FromResult(new LeaveClubResult());
    }
}