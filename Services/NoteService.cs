using NotesApi.Data;
using NotesApi.Dtos;
using NotesApi.Models;

namespace NotesApi.Services
{
    public interface INoteService
    {
        Task<IEnumerable<NoteDto>> GetNotes(Guid userId);
        Task<NoteDto?> GetNote(Guid id, Guid userId);
        Task<NoteDto> CreateNote(Guid userId, NoteRequestDto request);
        Task<bool> UpdateNote(Guid id, Guid userId, NoteRequestDto request);
        Task<bool> DeleteNote(Guid id, Guid userId);
    }

    public class NoteService : INoteService
    {
        private readonly NoteRepository _repo;

        public NoteService(NoteRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<NoteDto>> GetNotes(Guid userId) =>
            _repo.GetNotesByUserId(userId);

        public Task<NoteDto?> GetNote(Guid id, Guid userId) =>
            _repo.GetNoteById(id, userId);

        public async Task<NoteDto> CreateNote(Guid userId, NoteRequestDto request)
        {
            var now = DateTime.UtcNow;

            var note = new NoteDto
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = request.Title?.Trim() ?? string.Empty,
                Content = request.Content,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _repo.CreateNote(note);
            return note;
        }

        public async Task<bool> UpdateNote(Guid id, Guid userId, NoteRequestDto request)
        {
            var existing = await _repo.GetNoteById(id, userId);
            if (existing is null) return false;

            existing.Title = request.Title?.Trim() ?? existing.Title;
            existing.Content = request.Content;
            existing.UpdatedAt = DateTime.UtcNow;

            var rows = await _repo.UpdateNote(existing);
            return rows > 0;
        }

        public async Task<bool> DeleteNote(Guid id, Guid userId)
        {
            var rows = await _repo.DeleteNote(id, userId);
            return rows > 0;
        }
    }
}
