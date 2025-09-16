public class NoteDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }  // Optional
    public string Title { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}