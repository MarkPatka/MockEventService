using Mapster;
using UserService.Application.UserProfileManagement.Command.UpdateUserProfileCommand;
using UserService.Application.UserProfileManagement.Common;
using UserService.Application.UserProfileManagement.Queries.GetUserProfileQuery;
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
    }
}