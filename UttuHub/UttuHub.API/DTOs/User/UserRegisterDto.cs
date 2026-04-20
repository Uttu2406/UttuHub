namespace UttuHub.API.DTOs.User
{
    // UC 211 - Used for registering a new user (POST /users/register)
    // CHANGED: Added ImageUrl as optional field at registration
    public class RegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Plain text - will be hashed in controller
        public string? ImageUrl { get; set; }
    }
}

