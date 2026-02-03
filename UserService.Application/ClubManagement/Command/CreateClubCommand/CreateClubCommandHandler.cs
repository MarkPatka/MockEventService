using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Persistence;
using UserService.Application.Services;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Command.CreateClubCommand;

public class CreateClubCommandHandler : IRequestHandler<CreateClubCommand, CreateClubResult>
{
    private readonly IClubService _clubService;

    public CreateClubCommandHandler(IClubService clubService)
    {
        _clubService = clubService;
    }

    public async Task<CreateClubResult> Handle(CreateClubCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<Club> clubs = await _clubService
            .SearchClubsAsync(request.Name, null, OwnerId.Create(request.OwnerId))
            .ConfigureAwait(false);

        var club = clubs.ToList().FirstOrDefault();
        if (club != null)
        {
            throw new EntityNotFoundException("The same club exists");
        }

        club = Club.Create(
            request.Name,
            request.Description,
            request.Interests,
            OwnerId.Create(request.OwnerId),
            request.IsPublic,
            DateTime.UtcNow,
            DateTime.UtcNow);

        club = await _clubService.AddAsync(club).ConfigureAwait(true);

        return await Task.FromResult(
            new CreateClubResult(
                club.Id.Value,
                club.Name,
                club.Description,
                club.Owner.Value,
                club.IsPublic)
        );
    }
}