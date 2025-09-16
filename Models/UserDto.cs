public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? DisplayName { get; set; }
    public DateTime CreatedAt { get; set; }
}