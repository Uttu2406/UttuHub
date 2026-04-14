using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UttuHub.API.Models
{
    public class FeedItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? Created { get; set; }
        public bool IsHighlight { get; set; } = false;


        [Required]
        [JsonIgnore]
        public ICollection<FeedItemCategory> FeedItemCategories { get; set; } = new List<FeedItemCategory>();


        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public User? User { get; set; }

    }
}

