using UserService.Domain.Common.Abstract;

namespace UserService.Domain.UserProfileAggregate.ValueObjects;

public sealed class UserId : ValueObject
{
    public Guid Value { get; }

    private UserId(Guid value) => Value = value;

    public static UserId Create(Guid value) => new(value);
    public static UserId CreateUnique() => new(Guid.NewGuid());

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}


