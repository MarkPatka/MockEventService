using MockEventService.Domain.EventAggregate;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Application.Persistence.Specifications;

public class EventByIdSpec : BaseSpecification<Event>
{
    public EventByIdSpec(EventId eventId)
        : base(e => e.Id == eventId)
    {
        AddInclude(e => e.Participants);
        AddInclude(e => e.Reviews);
        ApplySplitQuery();
    }
}

