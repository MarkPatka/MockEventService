using Mapster;
using UserService.Application.ClubManagement.Command.CreateClubCommand;
using UserService.Application.ClubManagement.Command.DeleteClubCommand;
using UserService.Application.ClubManagement.Command.UpdateClubCommand;
using UserService.Application.ClubManagement.Common;
using UserService.Application.ClubManagement.Queries.GetClubQuery;
using UserService.Contracts.Clubs;

namespace UserService.Api.Mapping;

public class ClubMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetClubRequest, GetClubQuery>();
        config.NewConfig<GetClubResult, GetClubResponse>();

        config.NewConfig<CreateClubRequest, CreateClubCommand>();

        config.NewConfig<CreateClubResult, CreateClubResponse>();

        config.NewConfig<UpdateClubRequest, UpdateClubCommand>();
        config.NewConfig<UpdateClubResult, UpdateClubResponse>();

        config.NewConfig<DeleteClubRequest, DeleteClubCommand>();
        config.NewConfig<DeleteClubResult, DeleteClubResponse>();
    }
}