using Mapster;
using MockEventService.Application.EventManagement.Command.CreateEventCommand;
using MockEventService.Application.EventManagement.Common;
using MockEventService.Application.EventManagement.Queries.GetAllUserEventsQuery;
using MockEventService.Contracts.Events;

namespace MockEventService.Api.Mapping;

public class EventMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateEventRequest, CreateEventCommand>();

        config.NewConfig<CreateEventResult, CreateEventResponse>();

        config.NewConfig<GetAllUserEventsRequest, GetAllUserEventsQuery>();

        config.NewConfig<GetAllUserEventsResult, GetAllUserEventsResponse>();
        

    }
}