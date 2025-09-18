using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NotesApi.Models;
using NotesApi.Services;

namespace NotesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService) => _userService = userService;

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] AuthRegisterDto req)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = await _userService.Register(req);
            if (user == null) return BadRequest(new { message = "Email already exists." });

            return Ok(new { message = "User registered successfully." });
     
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthLoginRequestDto req)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _userService.Login(req.Email, req.Password);
            if (result == null) return Unauthorized(new { message = "Invalid credentials." });

            return Ok(result); // AuthLoginResponseDto
        }
    }
}
