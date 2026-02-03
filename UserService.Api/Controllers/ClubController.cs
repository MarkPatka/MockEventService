using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.ClubManagement.Command.CreateClubCommand;
using UserService.Application.ClubManagement.Command.DeleteClubCommand;
using UserService.Application.ClubManagement.Command.JoinClubCommand;
using UserService.Application.ClubManagement.Command.LeaveClubCommand;
using UserService.Application.ClubManagement.Command.UpdateClubCommand;
using UserService.Application.ClubManagement.Common;
using UserService.Application.ClubManagement.Queries.GetClubMembersQuery;
using UserService.Application.ClubManagement.Queries.GetClubQuery;
using UserService.Application.ClubManagement.Queries.GetClubsByUserQuery;
using UserService.Application.ClubManagement.Queries.SearchClubsQuery;
using UserService.Contracts.Clubs;

namespace UserService.Api.Controllers;

[Route("clubs")]
public class ClubController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;


    public ClubController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetClub(GetClubRequest request)
    {
        // request -> map to command
        var query = _mapper.Map<GetClubQuery>(request);

        // send command to request handler
        var result = await _sender.Send(query);

        // map the result model to response model 
        var response = _mapper.Map<GetClubResponse>(result);

        // get the handler response 
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClub([FromBody] CreateClubRequest request)
    {
        // request -> map to command
        var command = _mapper.Map<CreateClubCommand>(request);

        // send command to request handler
        var result = await _sender.Send(command);

        // map the result model to response model 
        var response = _mapper.Map<CreateClubResponse>(result);

        // get the handler response 
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateClub([FromBody] UpdateClubRequest request)
    {
        // request -> map to command
        var command = _mapper.Map<UpdateClubCommand>(request);

        // send command to request handler
        var result = await _sender.Send(command);

        // map the result model to response model 
        var response = _mapper.Map<UpdateClubResponse>(result);

        // get the handler response 
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteClub(DeleteClubRequest request)
    {
        // request -> map to command
        var command = _mapper.Map<DeleteClubCommand>(request);

        // send command to request handler
        var result = await _sender.Send(command);

        // map the result model to response model 
        var response = _mapper.Map<DeleteClubResponse>(result);

        // get the handler response 
        return Ok(response);
    }

    //TODO
    [HttpGet("owner")]
    public async Task<IActionResult> GetClubsByOwner(GetClubsByUserRequest request)
    {
        // request -> map to query
        var query = _mapper.Map<GetClubsByUserQuery>(request);

        // send command to request handler
        var result = await _sender.Send(query);

        // map the result model to response model 
        var response = _mapper.Map<IEnumerable<GetClubsByUserResponse>>(result);

        // get the handler response 
        return Ok(response);
    }
    
    [HttpGet("members")]
    public async Task<IActionResult> GetClubsMembers(GetClubMembersRequest request)
    {
        // request -> map to query
        var query = _mapper.Map<GetClubMembersQuery>(request);

        // send command to request handler
        var result = await _sender.Send(query);

        // map the result model to response model 
        var response = _mapper.Map<IEnumerable<GetClubMembersResponse>>(result);

        // get the handler response 
        return Ok(response);
    }

    [HttpPost("join")]
    public async Task<IActionResult> JoinToClub([FromBody] JoinToClubRequest request)
    {
        // request -> map to query
        var command = _mapper.Map<JoinToClubCommand>(request);

        // send command to request handler
        var result = await _sender.Send(command);

        // map the result model to response model 
        var response = _mapper.Map<JoinToClubResponse>(result);

        // get the handler response 
        return Ok(response);
    }

    [HttpPost("leave")]
    public async Task<IActionResult> LeaveClub([FromBody] LeaveClubRequest request)
    {
        // request -> map to query
        var command = _mapper.Map<LeaveClubCommand>(request);

        // send command to request handler
        var result = await _sender.Send(command);

        // map the result model to response model 
        var response = _mapper.Map<LeaveClubResponse>(result);

        // get the handler response 
        return Ok(response);
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchClubs([FromBody] SearchClubsRequest request)
    {
        // request -> map to query
        var query = _mapper.Map<SearchClubsQuery>(request);

        // send command to request handler
        var result = await _sender.Send(query);

        // map the result model to response model 
        var response = _mapper.Map<IEnumerable<SearchClubsResponse>>(result);

        // get the handler response 
        return Ok(response);
    }
}