using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.ClubManagement.Command.CreateClubCommand;
using UserService.Application.ClubManagement.Command.DeleteClubCommand;
using UserService.Application.ClubManagement.Command.UpdateClubCommand;
using UserService.Application.ClubManagement.Queries.GetClubQuery;
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
    public async Task<IActionResult> DeleteClub([FromBody] DeleteClubRequest request)
    {
        // request -> map to command
        var command = _mapper.Map<DeleteClubCommand>(request);

        // send command to request handler
        await _sender.Send(command);
        
        // get the handler response 
        return Ok();
    }
}