using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UttuHub.API.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? IconKey { get; set; }
        public string? HexColor { get; set; }


        [Required]
        [JsonIgnore]
        public ICollection<FeedItemCategory> FeedItemCategories { get; set; } = new List<FeedItemCategory>();
    }

}

