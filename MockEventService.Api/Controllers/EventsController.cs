using MediatR;
using Microsoft.AspNetCore.Mvc;
using MockEventService.Contracts.Event;

namespace MockEventService.Api.Controllers;

public class EventsController : ControllerBase
{
    private readonly ISender _sender;

    public EventsController(ISender sender)
    {
        _sender = sender;
    }



    [HttpPost("create")]
    public async Task<IActionResult> CreateEvent(CreateEventRequest request)
    {
        // request -> map to command

        // send command to request handler

        // get the handler response 

        // map the result model to responce model 
        await Task.CompletedTask;
        return Ok();
    }
}
