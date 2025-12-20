using MockEventService.Domain.Common.Abstract;
using MockEventService.Domain.EventAggregate.ValueObjects;

namespace MockEventService.Domain.EventAggregate.Entities;

public sealed class EventType : Entity<EventTypeId>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;

    private EventType() { }
    // TO FDO Create method
    public EventType(EventTypeId id) : base(id) { }
}
