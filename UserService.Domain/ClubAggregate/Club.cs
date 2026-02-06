using UserService.Domain.ClubAggregate.ValueObjects;
using UserService.Domain.Common.Abstract;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Domain.ClubAggregate;

public sealed class Club : AggregateRoot<ClubId>
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public OwnerId Owner { get; private set; }

    private List<string> _interests = new();
    public IReadOnlyCollection<string> Interests => _interests;

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
        IEnumerable<string> interests,
        OwnerId owner,
        bool isPublic,
        DateTime createdAt,
        DateTime? updatedAt) : base(id)
    {
        Id = id;
        Name = name;
        Description = description;
        _interests = (interests != null) ? interests.ToList() : new List<string>();
        Owner = owner;
        IsPublic = isPublic;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Club Create(
        string name,
        string description,
        IEnumerable<string> interests,
        OwnerId owner,
        bool isPublic,
        DateTime createdAt,
        DateTime? updatedAt)

    {
        return new Club(
            ClubId.CreateUnique(),
            name, description, interests, owner, isPublic, createdAt, updatedAt
        );
    }

    public static Club Update(
        Club club,
        string name,
        string description,
        IEnumerable<string> interests,
        OwnerId owner,
        bool isPublic,
        DateTime? updatedAt)

    {
        club.SetName(name);
        club.SetDescription(description);
        club.SetInterests(interests);
        club.SetOwner(owner);
        club.SetIsPublic(isPublic);
        club.UpdatedAt = updatedAt;
        return club;
    }

    private void SetName(string name)
    {
        Name = name;
    }

    private void SetDescription(string description)
    {
        Description = description;
    }

    private void SetIsPublic(bool isPublic)
    {
        IsPublic = isPublic;
    }

    private void SetInterests(IEnumerable<string> interests)
    {
        _interests = interests.ToList();
    }

    private void SetOwner(OwnerId owner)
    {
        Owner = owner;
    }

    public void AddMember(UserId userId, DateTime joinedAt)
    {
        if (_members.FirstOrDefault(m => m.UserId == userId) == null)
            _members.Add(ClubMember.Create(Id, userId, joinedAt));
    }

    public void DeleteMember(UserId userId, DateTime joinedAt)
    {
        var existMember = _members.FirstOrDefault(m => m.UserId == userId);
        if (existMember != null)
        {
            _members.Remove(existMember);
        }
    }
}