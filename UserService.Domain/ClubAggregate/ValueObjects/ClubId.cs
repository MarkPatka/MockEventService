using UserService.Domain.Common.Abstract;

namespace UserService.Domain.ClubAggregate.ValueObjects;

public sealed class ClubId : ValueObject
{
    public Guid Value { get; }

    private ClubId(Guid value) => Value = value;

    public static ClubId Create(Guid value) => new(value);
    public static ClubId CreateUnique() => new(Guid.NewGuid());

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}


