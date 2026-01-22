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

namespace UserService.Api.Mapping;

public class UserProfileMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetUserProfileRequest, GetUserProfileQuery>();
        config.NewConfig<GetUserProfileResult, GetUserProfileResponse>();

        config.NewConfig<UpdateUserProfileRequest, UpdateUserProfileCommand>();
        config.NewConfig<UpdateUserProfileResult, UpdateUserProfileResponse>();

        config.NewConfig<GetUserProfileInterestsRequest, GetUserProfileInterestsQuery>();
        config.NewConfig<GetUserProfileInterestsResult, GetUserProfileInterestsResponse>();

        config.NewConfig<UpdateUserProfileInterestsRequest, UpdateUserProfileInterestsCommand>();
        config.NewConfig<UpdateUserProfileInterestsResult, UpdateUserProfileInterestsResponse>();

        config.NewConfig<GetClubRequest, GetClubQuery>();
        config.NewConfig<GetClubResult, GetClubResponse>();

        config.NewConfig<CreateClubRequest, CreateClubCommand>();
        config.NewConfig<CreateClubResult, CreateClubResponse>();

        config.NewConfig<UpdateClubRequest, UpdateClubCommand>();
        config.NewConfig<UpdateClubResult, UpdateClubResponse>();
    }
}