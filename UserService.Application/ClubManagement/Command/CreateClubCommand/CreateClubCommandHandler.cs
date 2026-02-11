using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.IntegrationEvents;
using UserService.Application.IntegrationEvents.Mappers;
using UserService.Application.Persistence;
using UserService.Application.Services;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Command.CreateClubCommand;

public class CreateClubCommandHandler : IRequestHandler<CreateClubCommand, CreateClubResult>
{
    private readonly IClubService _clubService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIntegrationEventPublisher _publisher;

    public CreateClubCommandHandler(IClubService clubService, IUnitOfWork unitOfWork,
        IIntegrationEventPublisher publisher)
    {
        _clubService = clubService;
        _unitOfWork = unitOfWork;
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
        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

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