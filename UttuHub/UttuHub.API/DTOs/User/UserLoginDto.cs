namespace UttuHub.API.DTOs.User
{
    // UC 212 - Used for login (POST /users/login)
    // Separate from RegisterDto to keep intent clear
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Plain text - verified against hash in controller
    }
}