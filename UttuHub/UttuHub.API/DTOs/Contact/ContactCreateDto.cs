namespace UttuHub.API.DTOs.Contact
{
    // UC 241 - Used when a visitor posts a contact (POST /api/users/{username}/contacts)
    // CHANGED: Removed Id, IsApproved, SentAt, UserId - all set server-side
    // Address is optional
    public class ContactCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Message { get; set; }
        public string? Address { get; set; } // Optional field
        public bool IsPublic { get; set; } = true;
    }
}

