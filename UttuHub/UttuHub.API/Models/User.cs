using System.ComponentModel.DataAnnotations;
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
        public string? Email { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public bool isVerified { get; set; } = false;


        [JsonIgnore]
        public ICollection<Project>? Projects { get; set; }


        [JsonIgnore]
        public ICollection<FeedItem>? FeedItems { get; set; }


    }
}
