using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MockEventService.Application.EventManagement.Command.CreateEventCommand;
using MockEventService.Application.EventManagement.Common;
using MockEventService.Application.Persistence;
using MockEventService.Contracts.Event;
using MockEventService.Domain;
using System;

namespace MockEventService.Api.Controllers;

public class EventsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;


    public EventsController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }



    [HttpPost("create")]
    public async Task<IActionResult> CreateEvent(CreateEventRequest request)
    {
        // request -> map to command
        var command = _mapper.Map<CreateEventCommand>(request);

        // send command to request handler
        var result = await _sender.Send(command);


        // get the handler response 
        var response = _mapper.Map<CreateEventResponse>(result);

        // map the result model to responce model 
        return Ok(response);
    }
}
