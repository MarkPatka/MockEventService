using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Persistence;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Command.UpdateClubCommand;

public class UpdateClubCommandHandler : IRequestHandler<UpdateClubCommand, UpdateClubResult>
{
    private readonly IClubRepository _repository;

    public UpdateClubCommandHandler(IClubRepository repository)
    {
        _repository = repository;
    }

    public async Task<UpdateClubResult> Handle(UpdateClubCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<Club> clubs = await _repository
            .GetByFilterAsync(x =>
                x.Id.Value.ToString() == request.Name && x.Owner.Value.ToString() == request.OwnerId.ToString())
            .ConfigureAwait(false);

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

        club = await _repository.UpdateAsync(club).ConfigureAwait(true);

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