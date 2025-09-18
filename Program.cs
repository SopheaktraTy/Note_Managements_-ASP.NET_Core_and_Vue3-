using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using NotesApi.Data;
using NotesApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notes API", Version = "v1" });

    // Swagger "Authorize" button (Bearer JWT)
    c.AddSecurityDefinition("ApiBearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "JWT auth using the Bearer scheme. Example: \"Bearer {token}\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiBearerAuth" }
            },
            Array.Empty<string>()
        }
    });
});

// Dapper connection factory
builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

// Repositories
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<NoteRepository>();

// Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<INoteService, NoteService>();

// JWT configuration
var jwtSection = builder.Configuration.GetSection("Jwt");
var keyStr = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key not configured.");

    // Support BASE64: prefix; otherwise treat as raw UTF-8
byte[] keyBytes = keyStr.StartsWith("BASE64:", StringComparison.OrdinalIgnoreCase)
    ? Convert.FromBase64String(keyStr["BASE64:".Length..])
    : Encoding.UTF8.GetBytes(keyStr);

if (keyBytes.Length < 32) // HS256 needs >= 32 bytes (256 bits)
    throw new InvalidOperationException($"Jwt:Key must be >= 32 bytes. Current: {keyBytes.Length}.");

var signingKey = new SymmetricSecurityKey(keyBytes);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer   = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// This is Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();   // it need to me before UseAuthorization
app.UseAuthorization();

app.MapControllers();


app.MapFallbackToFile("index.html"); // SPA history fallback

app.Run();
