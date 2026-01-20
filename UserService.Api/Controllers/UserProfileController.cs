using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.UserProfileManagement.Command.UpdateUserProfileCommand;
using UserService.Application.UserProfileManagement.Command.UpdateUserProfileInterestsCommand;
using UserService.Application.UserProfileManagement.Common;
using UserService.Application.UserProfileManagement.Queries.GetUserProfileQuery;
using UserService.Contracts.UserProfiles;

namespace UserService.Api.Controllers;

public class UserProfileController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;


    public UserProfileController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("{userId}")]
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

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUserProfile(UpdateUserProfileRequest request)
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

    [HttpPut("interests/{userId}")]
    public async Task<IActionResult> UpdateUserProfileInterests(UpdateUserProfileInterestsRequest request)
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