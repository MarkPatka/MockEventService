using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Persistence;
using UserService.Domain.ClubAggregate;

namespace UserService.Application.ClubManagement.Command.CreateClubCommand;

public class CreateClubCommandHandler : IRequestHandler<CreateClubCommand, CreateClubResult>
{
    private readonly IClubRepository _repository;

    public CreateClubCommandHandler(IClubRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateClubResult> Handle(CreateClubCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<Club> clubs = await _repository
            .GetByFilterAsync(x =>
                x.Id.Value.ToString() == request.Name && x.Owner.Value.ToString() == request.OwnerId.ToString())
            .ConfigureAwait(false);

        var club = clubs.ToList().FirstOrDefault();
        if (club != null)
        {
            throw new EntityNotFoundException("The same club exists");
        }

        club = Club.Create(
            club.Name,
            club.Description,
            club.Owner,
            club.IsPublic,
            DateTime.Now,
            DateTime.Now);
        
        await _repository.AddAsync(club).ConfigureAwait(true);
        
        return await Task.FromResult(
            new CreateClubResult(
                club.Id,
                club.Name,
                club.Description,
                club.Owner,
                club.IsPublic)
        );
    }
}