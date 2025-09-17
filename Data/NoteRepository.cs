using Dapper;
using NotesApi.Models;

namespace NotesApi.Data
{
    public class NoteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public NoteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<NoteDto>> GetNotesByUserId(Guid userId)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<NoteDto>(
                "SELECT * FROM Notes WHERE UserId = @UserId ORDER BY CreatedAt DESC",
                new { UserId = userId });
        }

        public async Task<NoteDto?> GetNoteById(Guid id, Guid userId)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<NoteDto>(
                "SELECT * FROM Notes WHERE Id = @Id AND UserId = @UserId",
                new { Id = id, UserId = userId });
        }

        public async Task<int> CreateNote(NoteDto note)
        {
            using var conn = _connectionFactory.CreateConnection();
            var sql = @"INSERT INTO Notes (Id, UserId, Title, Content, CreatedAt, UpdatedAt)
                        VALUES (@Id, @UserId, @Title, @Content, @CreatedAt, @UpdatedAt)";
            return await conn.ExecuteAsync(sql, note);
        }

        public async Task<int> UpdateNote(NoteDto note)
        {
            using var conn = _connectionFactory.CreateConnection();
            var sql = @"UPDATE Notes SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt
                        WHERE Id = @Id AND UserId = @UserId";
            return await conn.ExecuteAsync(sql, note);
        }

        public async Task<int> DeleteNote(Guid id, Guid userId)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.ExecuteAsync("DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId", 
                                           new { Id = id, UserId = userId });
        }
    }
}
