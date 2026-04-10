using System.ComponentModel.DataAnnotations;

namespace UttuHub.API.Models
{
    public class Contact
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Message { get; set; }
        public string? Address { get; set; }

        [Required]
        public bool IsPublic { get; set; } = true;

        [Required]
        public bool IsApproved { get; set; } = false;

        public DateTime SentAt { get; set; } = DateTime.UtcNow; // Automatically set to current time when created

    }
}
