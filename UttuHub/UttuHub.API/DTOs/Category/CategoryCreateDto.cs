namespace UttuHub.API.DTOs.Category
{
    // UC 201 - Used when creating a new Category (POST /categories)
    // CHANGED: Removed Id - DB auto-generates it, client should never send it
    public class CategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? IconKey { get; set; }
        public string? HexColor { get; set; }
    }
}