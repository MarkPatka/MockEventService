using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.Common.Abstract;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Domain.ClubAggregate;

public sealed class Club : AggregateRoot<ClubId>
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public OwnerId Owner { get; private set; }
    public IReadOnlyList<UserId> Members { get; private set; } = new List<UserId>();
    public bool IsPublic { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Club()
    {
    }

    private Club(
        ClubId id,
        string name,
        string description,
        OwnerId owner,
        bool isPublic,
        DateTime createdAt,
        DateTime? updatedAt) : base(id)
    {
        Id = id;
        Name = name;
        Description = description;
        Owner = owner;
        IsPublic = isPublic;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Club Create(
        string name,
        string description,
        OwnerId owner,
        bool isPublic,
        DateTime createdAt,
        DateTime? updatedAt)

    {
        return new Club(
            ClubId.CreateUnique(),
            name, description, owner, isPublic, createdAt, updatedAt
        );
    }

    public static Club Create(
        ClubId id,
        string name,
        string description,
        OwnerId owner,
        bool isPublic,
        DateTime createdAt,
        DateTime? updatedAt)

    {
        return new Club(
            id, name, description, owner, isPublic, createdAt, updatedAt
        );
    }
}