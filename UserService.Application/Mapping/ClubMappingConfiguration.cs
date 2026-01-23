using Mapster;
using UserService.Application.ClubManagement.Command.CreateClubCommand;
using UserService.Application.ClubManagement.Command.UpdateClubCommand;
using UserService.Application.ClubManagement.Common;
using UserService.Application.ClubManagement.Queries.GetClubQuery;
using UserService.Application.UserProfileManagement.Command.UpdateUserProfileCommand;
using UserService.Application.UserProfileManagement.Command.UpdateUserProfileInterestsCommand;
using UserService.Application.UserProfileManagement.Common;
using UserService.Application.UserProfileManagement.Queries.GetUserProfileInterestsQuery;
using UserService.Application.UserProfileManagement.Queries.GetUserProfileQuery;
using UserService.Contracts.Clubs;
using UserService.Contracts.UserProfiles;
using UserService.Domain.ClubAggregate.ValueObjects;

namespace UserService.Application.Mapping;

public class ClubMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetClubRequest, GetClubQuery>();
        config.NewConfig<GetClubResult, GetClubResponse>();

        config.NewConfig<CreateClubRequest, CreateClubCommand>()
            .Map(dest => dest.OwnerId, src => OwnerId.Create(src.OwnerId));

        config.NewConfig<CreateClubResult, CreateClubResponse>();

        config.NewConfig<UpdateClubRequest, UpdateClubCommand>()
            .Map(dest => dest.OwnerId, src => OwnerId.Create(src.OwnerId));
        config.NewConfig<UpdateClubResult, UpdateClubResponse>();
    }
}