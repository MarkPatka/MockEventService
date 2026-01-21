using UserService.Domain.Common.Abstract;

namespace UserService.Domain.ClubAggregate.ValueObjects;

public sealed class OwnerId : ValueObject
{
    public Guid Value { get; }

    private OwnerId(Guid value) => Value = value;

    public static OwnerId Create(Guid value) => new(value);
    public static OwnerId CreateUnique() => new(Guid.NewGuid());
    public override string ToString() => $"{Value}";

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}


