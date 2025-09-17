using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using NotesApi.Data;
using NotesApi.Models;

namespace NotesApi.Services
{
    public class UserService
    {
        private readonly UserRepository _repo;
        private readonly IConfiguration _config;

        public UserService(UserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        // Register a new user
        public async Task<UserDto?> Register(AuthRegisterDto req)
        {
            var existing = await _repo.GetByEmail(req.Email);
            if (existing != null) return null;

            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Firstname = req.Firstname,
                Lastname  = req.Lastname,
                Email     = req.Email,
                PasswordHash = HashPassword(req.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _repo.CreateUser(user);
            return user;
        }

        // Login and return JWT + profile
        public async Task<AuthLoginResponseDto?> Login(string email, string password)
        {
            var user = await _repo.GetByEmail(email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            var token = GenerateJwtToken(user);
            return new AuthLoginResponseDto
            {
                Token     = token,
                Firstname = user.Firstname,
                Lastname  = user.Lastname,
                Email     = user.Email
            };
        }

        // === JWT creation ===
        private string GenerateJwtToken(UserDto user)
        {
            var keyStr = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var minutes = int.TryParse(_config["Jwt:ExpireMinutes"], out var m) ? m : 60;

            // Decode BASE64:... keys; else treat as UTF-8
            byte[] keyBytes = keyStr.StartsWith("BASE64:", StringComparison.OrdinalIgnoreCase)
                ? Convert.FromBase64String(keyStr["BASE64:".Length..])
                : Encoding.UTF8.GetBytes(keyStr);

            if (keyBytes.Length < 32)
                throw new InvalidOperationException("Jwt:Key must be at least 256 bits (32 bytes).");

            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // helpful for APIs
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("given_name", user.Firstname),
                new Claim("family_name", user.Lastname),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // === hashing helpers ===
        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private static bool VerifyPassword(string password, string storedHash) =>
            HashPassword(password) == storedHash;
    }
}
