using System.ComponentModel.DataAnnotations;

namespace UttuHub.API.DTOs.User
{
    // UC 211 - Used for registering a new user (POST /users/register)
    // CHANGED: Added ImageUrl as optional field at registration
    public class RegisterDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty; // Plain text - will be hashed in controller
        public string? ImageUrl { get; set; }
    }
}

