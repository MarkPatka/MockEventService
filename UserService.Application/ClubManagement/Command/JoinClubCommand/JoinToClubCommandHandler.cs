using MediatR;
using UserService.Application.ClubManagement.Common;
using UserService.Application.Common.Exceptions;
using UserService.Application.Persistence;
using UserService.Domain.ClubAggregate;
using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.ClubManagement.Command.JoinClubCommand;

public class JoinToClubCommandHandler : IRequestHandler<JoinToClubCommand, JoinToClubResult>
{
    private readonly IClubMembersRepository _repository;

    public JoinToClubCommandHandler(IClubMembersRepository repository)
    {
        _repository = repository;
    }

    public async Task<JoinToClubResult> Handle(JoinToClubCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<ClubMember> clubs = await _repository
            .GetByFilterAsync(x => x.ClubId.Value == request.ClubId && x.UserId.Value == request.UserId)
            .ConfigureAwait(false);
        
        if (clubs?.Count() > 0)
        {
            throw new AlreadyDoneException("You have already joined the club");
        }

        var clubMember = ClubMember.Create(ClubId.Create(request.ClubId), UserId.Create(request.UserId), DateTime.Now);
        await _repository.AddAsync(clubMember).ConfigureAwait(true);
        
        return await Task.FromResult(new JoinToClubResult());
    }
}