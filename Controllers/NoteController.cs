using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Dtos;
using NotesApi.Services;
using System.Security.Claims;

namespace NotesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        private Guid GetUserId()
            {
                var userIdStr =
                    User.FindFirstValue(ClaimTypes.NameIdentifier)   // mapped from "sub"
                    ?? User.FindFirstValue("sub")                    // raw claim
                    ?? User.FindFirstValue("uid")                    // some IdPs
                    ?? User.FindFirstValue("user_id");               // some IdPs

                if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                    throw new UnauthorizedAccessException(
                        "User id claim not found or invalid. Expected 'sub' or 'nameidentifier'.");

                return userId;
            }


        [HttpGet("ViewNotes")]
        public async Task<IActionResult> GetNotes()
        {
            var userId = GetUserId();

            var notes = await _noteService.GetNotes(userId);

            var response = notes.Select(n => new NoteResponseDto
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                CreatedAt = n.CreatedAt,
                UpdatedAt = n.UpdatedAt
            });

            return Ok(response);
        }

        [HttpGet("ViewNotesByID/{id:guid}")]
        public async Task<IActionResult> GetNoteById(Guid id)
        {
            var userId = GetUserId();
            var note = await _noteService.GetNote(id, userId);
            if (note is null) return NotFound();

            var response = new NoteResponseDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };

            return Ok(response);
        }

        [HttpPost("CreateNotes")]
        public async Task<IActionResult> CreateNote([FromBody] NoteRequestDto request)
        {
            if (request is null) return BadRequest("Body required.");
            if (string.IsNullOrWhiteSpace(request.Title))
                return ValidationProblem("Title is required.");

            var userId = GetUserId();
            var created = await _noteService.CreateNote(userId, request);

            var response = new NoteResponseDto
            {
                Id = created.Id,
                Title = created.Title,
                Content = created.Content,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };

            return CreatedAtAction(nameof(GetNoteById), new { id = response.Id }, response);
        }

        [HttpPut("UpdateNotes/{id:guid}")]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] NoteRequestDto request)
        {
            if (request is null) return BadRequest("Body required.");
            if (string.IsNullOrWhiteSpace(request.Title))
                return ValidationProblem("Title is required.");

            var userId = GetUserId();
            var ok = await _noteService.UpdateNote(id, userId, request);
            if (!ok) return NotFound();

            return NoContent();
        }

        [HttpDelete("DeleteNotes/{id:guid}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            var userId = GetUserId();
            var ok = await _noteService.DeleteNote(id, userId);
            if (!ok) return NotFound();

            return NoContent();
        }
    }
}
