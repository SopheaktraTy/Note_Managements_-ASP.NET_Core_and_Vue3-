using Dapper;
using NotesApi.Models;

namespace NotesApi.Data
{
    public class UserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<UserDto?> GetByEmail(string email)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<UserDto>(
                "SELECT * FROM Users WHERE Email = @Email",
                new { Email = email });
        }

        public async Task<UserDto?> GetById(Guid id)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<UserDto>(
                "SELECT * FROM Users WHERE Id = @Id",
                new { Id = id });
        }

        public async Task<int> CreateUser(UserDto user)
        {
            using var conn = _connectionFactory.CreateConnection();
            var sql = @"INSERT INTO Users (Id, Firstname, Lastname, Email, PasswordHash, CreatedAt)
                        VALUES (@Id, @Firstname, @Lastname, @Email, @PasswordHash, @CreatedAt)";
            return await conn.ExecuteAsync(sql, user);
        }
    }
}
