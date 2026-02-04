namespace UserService.Application.ClubManagement.Dto;

public class ClubDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public bool IsPublic { get; init; }
    public DateTime CreatedAt { get; init; }
}