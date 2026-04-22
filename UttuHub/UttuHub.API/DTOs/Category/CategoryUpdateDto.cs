namespace UttuHub.API.DTOs.Category
{
    // UC 203 - Used when updating a Category (PUT /categories/{id})
    // CHANGED: Removed Id from request body - taken from route parameter instead
    public class CategoryUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? IconKey { get; set; }
        public string? HexColor { get; set; }
    }
}

