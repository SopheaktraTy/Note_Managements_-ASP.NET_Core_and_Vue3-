namespace NotesApi.Dtos
{
    public class NoteRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
    }
}
