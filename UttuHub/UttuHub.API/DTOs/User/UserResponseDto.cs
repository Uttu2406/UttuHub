namespace UttuHub.API.DTOs.User
{
    // DTO for returning User data - excludes PasswordHash for safety
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsVerified { get; set; }
    }
}