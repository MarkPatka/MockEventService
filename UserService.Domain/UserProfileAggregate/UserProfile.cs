using UserService.Domain.Common.Abstract;
using UserService.Domain.UserProfileAggregate.ValueObjects;

namespace UserService.Domain.UserProfileAggregate;

public sealed class UserProfile : AggregateRoot<UserId>
{
    public string DisplayName { get; private set; } = string.Empty;
    public string Bio { get; private set; } = string.Empty;
    public Uri? AvatarUri { get; private set; }
    private List<string> _interests = new();
    public IReadOnlyList<string> Interests => _interests;
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
        DisplayName = displayName;
        Bio = bio;
        AvatarUri = avatarUri;
        _interests = interests.ToList();
        BirthDate = birthDate;
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

    public static UserProfile Update(
        UserProfile userProfile,
        string displayName,
        string bio,
        Uri? avatarUri,
        IReadOnlyList<string> interests,
        DateTime? birthDate,
        DateTime? updatedAt)
    {
        userProfile.SetBirthDate(birthDate);
        userProfile.SetInterests(interests);
        userProfile.SetAvatarUri(avatarUri);
        userProfile.SetBio(bio);
        userProfile.SetDisplayName(displayName);
        userProfile.UpdatedAt = updatedAt;
        return userProfile;
    }
    
    private void SetBirthDate(DateTime? birthDate)
    {
        BirthDate = birthDate;
    }
    
    private void SetInterests(IReadOnlyList<string> interests)
    {
        _interests =  interests.ToList();
    }
    
    private void SetAvatarUri(Uri? avatarUri)
    {
        AvatarUri = avatarUri;
    }
    
    private void SetBio(string bio)
    {
        Bio = bio;
    }
    
    private void SetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }
}