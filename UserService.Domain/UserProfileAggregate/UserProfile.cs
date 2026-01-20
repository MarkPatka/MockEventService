using UserService.Domain.Common.Abstract;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Domain.UserProfileAggregate;

public sealed class UserProfile : AggregateRoot<UserId>
{
    public string DisplayName { get; private set; } = string.Empty;
    public string Bio { get; private set; } = string.Empty;
    public Uri? AvatarUri { get; private set; }
    public IReadOnlyList<string> Interests { get; private set; } = new List<string>();
    public DateTime? BirthDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private UserProfile()
    {
    }

    private UserProfile(
        UserId id,
        string displayName,
        string bio,
        Uri? avatarUri,
        IReadOnlyList<string> interests,
        DateTime? birthDate,
        DateTime createdAt,
        DateTime? updatedAt
    )
        : base(id)
    {
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static UserProfile Create(
        string displayName,
        string bio,
        Uri? avatarUri,
        IReadOnlyList<string> interests,
        DateTime? birthDate,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        return new UserProfile(
            UserId.CreateUnique(),
            displayName,
            bio,
            avatarUri,
            interests,
            birthDate,
            createdAt,
            updatedAt);
    }
    
    public static UserProfile Create(
        UserId id,
        string displayName,
        string bio,
        Uri? avatarUri,
        IReadOnlyList<string> interests,
        DateTime? birthDate,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        return new UserProfile(
            id,
            displayName,
            bio,
            avatarUri,
            interests,
            birthDate,
            createdAt,
            updatedAt);
    }
}