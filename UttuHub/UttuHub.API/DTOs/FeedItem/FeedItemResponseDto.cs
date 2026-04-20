using UttuHub.API.DTOs.Category;

namespace UttuHub.API.DTOs.FeedItem
{
    // UC 221.1, 222 - Returned when fetching FeedItem(s)
    // Resolves categories into readable objects instead of raw FKs
    public class FeedItemResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? Created { get; set; }
        public bool IsHighlight { get; set; }
        public int UserId { get; set; }


        public List<CategorySummaryDto> Categories { get; set; } = new();
    }
}