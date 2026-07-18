using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UttuHub.API.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        [Column("isVerified")]
        public bool IsVerified { get; set; } = false;


        [JsonIgnore]
        public ICollection<Project>? Projects { get; set; }


        [JsonIgnore]
        public ICollection<FeedItem>? FeedItems { get; set; }


        [JsonIgnore]
        public ICollection<Contact>? Contacts { get; set; }


    }
}
