namespace UttuHub.API.DTOs.FeedItem
{
    // UC 221 - Used when creating a new FeedItem (POST /feeditems)
    // CategoryIds links existing categories via junction table
    // Created is set server-side, not taken from client
    public class FeedItemCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsHighlight { get; set; }
        public int UserId { get; set; }


        public List<int> CategoryIds { get; set; } = new(); // IDs of categories to link
    }
}

