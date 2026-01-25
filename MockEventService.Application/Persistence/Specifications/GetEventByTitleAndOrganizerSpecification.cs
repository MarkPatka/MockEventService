using MockEventService.Application.EventManagement.Command.CreateEventCommand;
using MockEventService.Domain.EventAggregate;
using MockEventService.Domain.EventAggregate.Enumerations;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Application.Persistence.Specifications;

public class GetEventByTitleAndOrganizerSpecification : BaseSpecification<Event>
{
    public GetEventByTitleAndOrganizerSpecification(string title, OrganizerId OrganizerId)
    {
        //    x.EventType == request.EventType &&
        //    x.OrganizerId == request.OrganizerId
        AddCriteria(x => x.Title == title);
        AddCriteria(x => x.OrganizerId == OrganizerId);
        ApplyNoTracking();
    }
}
