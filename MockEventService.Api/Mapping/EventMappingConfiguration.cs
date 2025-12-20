using Mapster;
using MockEventService.Application.EventManagement.Command.CreateEventCommand;
using MockEventService.Application.EventManagement.Common;
using MockEventService.Contracts.Event;

namespace MockEventService.Api.Mapping;

public class EventMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateEventRequest, CreateEventCommand>();

        config.NewConfig<CreateEventResult, CreateEventResponse>();
        

    }
}