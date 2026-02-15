using Mapster;
using MockEventService.Application.EventManagement.Command.CreateEventCommand;
using MockEventService.Application.EventManagement.Common;
using MockEventService.Application.EventManagement.Queries.GetAllUserEventsQuery;
using MockEventService.Contracts.DTO;
using MockEventService.Contracts.Events;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Api.Mapping;

public class EventMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Map using constructors for immutable types
        config.NewConfig<LocationFullDto, Location>()
            .ConstructUsing(src => Location.Create(
                src.Address,
                src.City,
                src.Country,
                src.Latitude,
                src.Longitude
            ));

        config.NewConfig<CreateEventRequest, CreateEventCommand>()
            .ConstructUsing(src => new CreateEventCommand(
                src.Title,
                src.EventTypeId,
                src.StartDate,
                src.EndDate,
                src.MaxParticipants,
                src.OrganizerId,
                src.Description,
                MapLocation(src.Location)
            ));

        config.NewConfig<CreateEventResult, CreateEventResponse>();

        config.NewConfig<GetAllUserEventsRequest, GetAllUserEventsQuery>();

        config.NewConfig<GetAllUserEventsResult, GetAllUserEventsResponse>();
    }

    private static Location MapLocation(LocationFullDto dto) =>
        Location.Create(
            dto.Address,
            dto.City,
            dto.Country,
            dto.Latitude,
            dto.Longitude
        );
}