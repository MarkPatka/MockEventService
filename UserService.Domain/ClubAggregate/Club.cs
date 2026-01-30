using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.Common.Abstract;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Domain.ClubAggregate;

public sealed class Club : AggregateRoot<ClubId>
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public OwnerId Owner { get; private set; }
    public IEnumerable<string> Interests { get; private set; } = new List<string>();

    private readonly List<ClubMember> _members = [];
    public IReadOnlyCollection<ClubMember> ClubMembers => _members.AsReadOnly();

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

    public void AddMember(UserId userId, DateTime joinedAt)
    {
        if (_members.FirstOrDefault(m => m.UserId == userId) == null)
            _members.Add(ClubMember.Create(Id, userId, joinedAt));
    }
    
    public void DeleteMember(UserId userId, DateTime joinedAt)
    {
        if (_members.FirstOrDefault(m => m.UserId == userId) == null)
            _members.Remove(ClubMember.Create(Id, userId, joinedAt));
    }
    
}