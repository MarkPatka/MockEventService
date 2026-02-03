using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Persistence;
using UserService.Application.Services;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Command.JoinClubCommand;

public class JoinToClubCommandHandler : IRequestHandler<JoinToClubCommand, JoinToClubResult>
{
    private readonly IClubService _clubService;

    public JoinToClubCommandHandler(IClubService clubService)
    {
        _clubService = clubService;
    }

    public async Task<JoinToClubResult> Handle(JoinToClubCommand request, CancellationToken cancellationToken)
    {
        var clubId = ClubId.Create(request.ClubId);
        var club = await _clubService.GetClubByIdAsync(clubId).ConfigureAwait(false);
        if (club == null)
        {
            throw new EntityNotFoundException("Club not found");
        }

        var joinedAt = DateTime.UtcNow;
        await _clubService.AddMember(club, UserId.Create(request.UserId), joinedAt).ConfigureAwait(false);
        return await Task.FromResult(new JoinToClubResult(request.ClubId, request.UserId, joinedAt));
    }
}