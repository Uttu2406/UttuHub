using System.ComponentModel.DataAnnotations;

namespace UttuHub.API.DTOs.User
{
    // UC 216 - Used when updating user info (PUT /users/{id})
    // Password is optional - only updated if provided
    // IsVerified is NOT updatable here - use verify endpoint instead
    public class UserUpdateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [EmailAddress]
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }
        [MinLength(6)]
        public string? Password { get; set; } // Optional - only re-hashed and saved if provided
    }
}