namespace UttuHub.API.DTOs.Contact
{
    // UC 241.1, 242 - Returned when fetching Contact(s)
    // Contact has no Create DTO - IsApproved and UserId are set server-side
    // The only update is ApproveContact which takes no body (just the ID)
    public class ContactResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Message { get; set; }
        public string? Address { get; set; } // Optional field
        public bool IsPublic { get; set; }
        public bool IsApproved { get; set; }
        public DateTime SentAt { get; set; }
        public int UserId { get; set; }
    }
}


