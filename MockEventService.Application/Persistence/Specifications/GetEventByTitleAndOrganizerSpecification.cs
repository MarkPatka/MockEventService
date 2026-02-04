using MockEventService.Domain.EventAggregate;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Application.Persistence.Specifications;

public class GetEventByTitleAndOrganizerSpecification : BaseSpecification<Event>
{
    public GetEventByTitleAndOrganizerSpecification(string title, UserId OrganizerId)
    {
        AddCriteria(x => x.Title == title);
        AddCriteria(x => x.OrganizerId == OrganizerId);
        ApplyNoTracking();
    }
}
