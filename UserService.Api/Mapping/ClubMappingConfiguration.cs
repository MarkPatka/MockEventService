using Mapster;
using UserService.Application.ClubManagement.Command.CreateClubCommand;
using UserService.Application.ClubManagement.Command.DeleteClubCommand;
using UserService.Application.ClubManagement.Command.JoinClubCommand;
using UserService.Application.ClubManagement.Command.LeaveClubCommand;
using UserService.Application.ClubManagement.Command.UpdateClubCommand;
using UserService.Application.ClubManagement.Common;
using UserService.Application.ClubManagement.Queries.GetClubMembersQuery;
using UserService.Application.ClubManagement.Queries.GetClubQuery;
using UserService.Application.ClubManagement.Queries.GetClubsByUserQuery;
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

        config.NewConfig<GetClubsByUserRequest, GetClubsByUserQuery>();
        config.NewConfig<GetClubsByUserResult, GetClubsByUserResponse>();

        config.NewConfig<GetClubMembersRequest, GetClubMembersQuery>();
        config.NewConfig<GetClubMembersResult, GetClubMembersResponse>();
        
        config.NewConfig<JoinToClubRequest, JoinToClubCommand>();
        config.NewConfig<JoinToClubResult, JoinToClubResponse>();
        
        config.NewConfig<LeaveClubRequest, LeaveClubCommand>();
        config.NewConfig<LeaveClubResult, LeaveClubResponse>();
    }
}