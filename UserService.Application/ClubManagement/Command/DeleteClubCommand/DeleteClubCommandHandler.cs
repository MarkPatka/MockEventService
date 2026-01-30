using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Persistence;
using UserService.Application.Services;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Command.DeleteClubCommand;

public class DeleteClubCommandHandler : IRequestHandler<DeleteClubCommand, DeleteClubResult>
{
    private readonly IClubService _clubService;

    public DeleteClubCommandHandler(IClubService clubService)
    {
        _clubService = clubService;
    }

    public async Task<DeleteClubResult> Handle(DeleteClubCommand request, CancellationToken cancellationToken)
    {
        Club? club = await _clubService.GetClubByIdAsync(ClubId.Create(request.id)).ConfigureAwait(false);

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

        await _clubService.UpdateAsync(club).ConfigureAwait(true);

        return await Task.FromResult(new DeleteClubResult());
    }
}