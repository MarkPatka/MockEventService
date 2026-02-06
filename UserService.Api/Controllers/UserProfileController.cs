using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.UserProfileManagement.Command.CreateUserProfileCommand;
using UserService.Application.UserProfileManagement.Command.UpdateUserProfileCommand;
using UserService.Application.UserProfileManagement.Command.UpdateUserProfileInterestsCommand;
using UserService.Application.UserProfileManagement.Common;
using UserService.Application.UserProfileManagement.Queries.GetUserProfileInterestsQuery;
using UserService.Application.UserProfileManagement.Queries.GetUserProfileQuery;
using UserService.Contracts.UserProfiles;

namespace UserService.Api.Controllers;

[Route("profiles")]
public class UserProfileController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;


    public UserProfileController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserProfile(GetUserProfileRequest request)
    {
        // request -> map to command
        var query = _mapper.Map<GetUserProfileQuery>(request);

        // send command to request handler
        var result = await _sender.Send(query);

        // map the result model to response model 
        var response = _mapper.Map<GetUserProfileResponse>(result);

        // get the handler response 
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUserProfile([FromBody] CreateUserProfileRequest request)
    {
        // request -> map to command
        var command = _mapper.Map<CreateUserProfileCommand>(request);

        // send command to request handler
        var result = await _sender.Send(command);

        // map the result model to response model 
        var response = _mapper.Map<CreateUserProfileResponse>(result);

        // get the handler response 
        return Ok(response);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileRequest request)
    {
        // request -> map to command
        var command = _mapper.Map<UpdateUserProfileCommand>(request);

        // send command to request handler
        var result = await _sender.Send(command);

        // map the result model to response model 
        var response = _mapper.Map<UpdateUserProfileResult>(result);

        // get the handler response 
        return Ok(response);
    }
    
    [HttpGet("interests")]
    public async Task<IActionResult> GetUserProfileInterests(GetUserProfileInterestsRequest request)
    {
        // request -> map to command
        var query = _mapper.Map<GetUserProfileInterestsQuery>(request);

        // send command to request handler
        var result = await _sender.Send(query);

        // map the result model to response model 
        var response = _mapper.Map<GetUserProfileInterestsResult>(result);

        // get the handler response 
        return Ok(response);
    }
    
    [HttpPut("interests")]
    public async Task<IActionResult> UpdateUserProfileInterests([FromBody] UpdateUserProfileInterestsRequest request)
    {
        // request -> map to command
        var command = _mapper.Map<UpdateUserProfileInterestsCommand>(request);

        // send command to request handler
        var result = await _sender.Send(command);

        // map the result model to response model 
        var response = _mapper.Map<UpdateUserProfileInterestsResult>(result);

        // get the handler response 
        return Ok(response);
    }
}