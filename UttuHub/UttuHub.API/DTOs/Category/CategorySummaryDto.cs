namespace UttuHub.API.DTOs.Category
{
    // Shared DTO - used inside FeedItemResponseDto
    // Lightweight category representation (no FeedItemCategories collection)
    public class CategorySummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? HexColor { get; set; }
        public string? IconKey { get; set; }
    }
}

