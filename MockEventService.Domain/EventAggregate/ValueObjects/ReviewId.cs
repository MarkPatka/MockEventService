using MockEventService.Domain.Common.Abstract;

namespace MockEventService.Domain.EventAggregate.ValueObjects;

public sealed class ReviewId : ValueObject, IEntityId
{
    public Guid Value { get; }

    object IEntityId.Value => Value;

    private ReviewId(Guid value) => Value = value;

    public static ReviewId Create(Guid value) => new(value);
    public static ReviewId CreateUnique() => new(Guid.NewGuid());

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
