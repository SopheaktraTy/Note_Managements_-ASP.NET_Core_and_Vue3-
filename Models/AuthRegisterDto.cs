using System.ComponentModel.DataAnnotations;
namespace NotesApi.Models
{
 public class AuthRegisterDto
    {
        [Required]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        public string Lastname  { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email     { get; set; } = string.Empty;

        // Strong password: ≥8 chars, 1 upper, 1 lower, 1 digit, 1 special symbol
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters and include an uppercase letter, a lowercase letter, a number, and a special symbol.")]
        public string Password  { get; set; } = string.Empty;
    }
}
