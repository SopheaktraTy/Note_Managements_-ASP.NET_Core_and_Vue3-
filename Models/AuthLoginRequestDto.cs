using System.ComponentModel.DataAnnotations;
namespace NotesApi.Models
{
    public class AuthLoginRequestDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
