public class CreateNoteDto
{
    public Guid? UserId { get; set; } // Optional
    public string Title { get; set; }
    public string? Content { get; set; }
}