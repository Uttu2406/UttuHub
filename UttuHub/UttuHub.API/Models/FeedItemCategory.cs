namespace UttuHub.API.Models
{
    public class FeedItemCategory
    {
        public int FeedItemId { get; set; }
        public FeedItem? FeedItem { get; set; }


        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}

