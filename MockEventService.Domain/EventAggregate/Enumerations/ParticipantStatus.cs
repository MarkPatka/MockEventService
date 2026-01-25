using MockEventService.Domain.Common.Abstract;

namespace MockEventService.Domain.EventAggregate.Enumerations;

public sealed class ParticipantStatus : Enumeration
{
    public static readonly ParticipantStatus Registered = new(1, nameof(Registered), "Зарегистрирован");
    public static readonly ParticipantStatus Attended   = new(2, nameof(Attended), "Присутствовал");
    public static readonly ParticipantStatus Cancelled  = new(3, nameof(Cancelled), "Отменил участие");

    private ParticipantStatus(int id, string name, string? description = null)
        : base(id, name, description) { }
}
