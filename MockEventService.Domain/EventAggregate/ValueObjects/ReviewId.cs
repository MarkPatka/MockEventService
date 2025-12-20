using MockEventService.Domain.Common.Abstract;

namespace MockEventService.Domain.EventAggregate.ValueObjects;

public sealed class ReviewId : ValueObject
{
    public Guid Value { get; }

    private ReviewId(Guid value) => Value = value;

    public static ReviewId Create(Guid value) => new(value);
    public static ReviewId CreateUnique() => new(Guid.NewGuid());

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
