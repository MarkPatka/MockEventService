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
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Application.Mapping;

public class UserProfileMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetUserProfileRequest, GetUserProfileQuery>();
        config.NewConfig<GetUserProfileResult, GetUserProfileResponse>();

        config.NewConfig<UpdateUserProfileRequest, UpdateUserProfileCommand>()
            .Map(dest => dest.Id, src => UserId.Create(src.id));
        config.NewConfig<UpdateUserProfileResult, UpdateUserProfileResponse>();

        config.NewConfig<GetUserProfileInterestsRequest, GetUserProfileInterestsQuery>();
        config.NewConfig<GetUserProfileInterestsResult, GetUserProfileInterestsResponse>();

        config.NewConfig<UpdateUserProfileInterestsRequest, UpdateUserProfileInterestsCommand>()
            .Map(dest => dest.Id, src => UserId.Create(src.id));
        config.NewConfig<UpdateUserProfileInterestsResult, UpdateUserProfileInterestsResponse>();
    }
}