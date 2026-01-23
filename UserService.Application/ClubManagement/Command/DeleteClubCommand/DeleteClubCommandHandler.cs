using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Persistence;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Command.DeleteClubCommand;

public class DeleteClubCommandHandler : IRequestHandler<DeleteClubCommand, DeleteClubResult>
{
    private readonly IClubRepository _repository;

    public DeleteClubCommandHandler(IClubRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeleteClubResult> Handle(DeleteClubCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<Club> clubs = await _repository
            .GetByFilterAsync(x => x.Id.Value.ToString() == request.id.ToString())
            .ConfigureAwait(false);

        var club = clubs.ToList().FirstOrDefault();
        if (club == null)
        {
            throw new EntityNotFoundException("Club doesn't exist");
        }

        club = Club.Create(
            ClubId.Create(request.id),
            club.Name,
            club.Description,
            club.Owner,
            club.IsPublic,
            DateTime.Now,
            DateTime.Now);

        await _repository.DeleteAsync(club).ConfigureAwait(true);

        return await Task.FromResult(new DeleteClubResult());
    }
}