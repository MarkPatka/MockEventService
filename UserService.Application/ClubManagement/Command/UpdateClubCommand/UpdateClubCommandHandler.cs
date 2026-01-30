using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Persistence;
using UserService.Application.Services;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Command.UpdateClubCommand;

public class UpdateClubCommandHandler : IRequestHandler<UpdateClubCommand, UpdateClubResult>
{
    private readonly IClubService _clubService;

    public UpdateClubCommandHandler(IClubService clubService)
    {
        _clubService = clubService;
    }

    public async Task<UpdateClubResult> Handle(UpdateClubCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<Club> clubs = await _clubService
            .SearchClubsAsync(request.Name, null, OwnerId.Create(request.OwnerId)).ConfigureAwait(false);

        var club = clubs.ToList().FirstOrDefault();
        if (club == null)
        {
            throw new EntityNotFoundException("The doesn't exist");
        }

        club = Club.Create(
            club.Id,
            request.Name,
            request.Description,
            OwnerId.Create(request.OwnerId),
            request.IsPublic,
            DateTime.Now,
            DateTime.Now);

        await _clubService.UpdateAsync(club).ConfigureAwait(true);

        return await Task.FromResult(
            new UpdateClubResult(
                club.Id.Value,
                club.Name,
                club.Description,
                club.Owner.Value,
                club.IsPublic)
        );
    }
}