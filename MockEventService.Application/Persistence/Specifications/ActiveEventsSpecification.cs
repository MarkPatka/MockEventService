using MockEventService.Domain.EventAggregate;
using MockEventService.Domain.EventAggregate.Enumerations;

namespace MockEventService.Application.Persistence.Specifications;

public class ActiveEventsSpecification : BaseSpecification<Event>
{
    public ActiveEventsSpecification(DateTime fromDate)
    {
        AddCriteria(e => e.Status == EventStatus.Active);
        AddCriteria(e => e.StartDate >= fromDate);
        ApplyOrderBy(e => e.StartDate);
        ApplyNoTracking();
    }
}
