namespace UttuHub.API.DTOs.FeedItem
{
    // UC 223 - Used when updating an existing FeedItem (PUT /feeditems/{id})
    // CategoryIds replaces ALL existing category links (delete old, insert new)
    // UserId and Created are NOT updatable - protected server-side
    public class FeedItemUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsHighlight { get; set; }


        public List<int> CategoryIds { get; set; } = new(); // Replaces all existing category links
    }
}