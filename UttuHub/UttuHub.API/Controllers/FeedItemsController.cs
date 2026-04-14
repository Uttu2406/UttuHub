using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UttuHub.API.Data;
using UttuHub.API.Models;

namespace UttuHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public FeedItemsController(AppDbContext context) { _context = context; }

        // UC 221 - Create FeedItem with Multiple Categories
        [HttpPost]
        public async Task<ActionResult> PostFeedItem(FeedItemCreateDto dto)
        {
            var feedItem = new FeedItem
            {
                Title = dto.Title,
                Content = dto.Content,
                ImageUrl = dto.ImageUrl,
                IsHighlight = dto.IsHighlight,
                UserId = dto.UserId,
                Created = DateTime.UtcNow
            };

            if (dto.CategoryIds != null && dto.CategoryIds.Any())
            {
                foreach (var catId in dto.CategoryIds)
                {
                    feedItem.FeedItemCategories.Add(new FeedItemCategory { CategoryId = catId });
                }
            }

            _context.FeedItems.Add(feedItem);
            await _context.SaveChangesAsync();

            var result = new
            {
                feedItem.Id,
                feedItem.Title,
                CategoryCount = dto.CategoryIds?.Count ?? 0
            };

            return CreatedAtAction(nameof(PostFeedItem), new { id = feedItem.Id }, result);
        }

        // UC 222 - Read all FeedItems (Including Categories)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetFeedItems()
        {
            return await _context.FeedItems
                .Include(f => f.FeedItemCategories)
                    .ThenInclude(fc => fc.Category)
                .OrderByDescending(f => f.Created)
                .Select(f => new {
                    f.Id,
                    f.Title,
                    f.Content,
                    f.ImageUrl,
                    f.Created,
                    f.IsHighlight,
                    f.UserId,
                    
                    Categories = f.FeedItemCategories.Select(fc => new {
                        fc.Category!.Id,
                        fc.Category.Name,
                        fc.Category.HexColor,
                        fc.Category.IconKey
                    }).ToList()
                })
                .ToListAsync();
        }

        // UC 223 - Update FeedItem
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedItem(int id, FeedItem feedItem)
        {
            if (id != feedItem.Id) return BadRequest("ID mismatch");

            _context.Entry(feedItem).State = EntityState.Modified;
            _context.Entry(feedItem).Property(x => x.Created).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FeedItems.Any(e => e.Id == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // UC 224 - Delete FeedItem
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedItem(int id)
        {
            var feedItem = await _context.FeedItems.FindAsync(id);
            if (feedItem == null) return NotFound();

            _context.FeedItems.Remove(feedItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    // DTO !!!
    public class FeedItemCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsHighlight { get; set; }
        public int UserId { get; set; }
        public List<int> CategoryIds { get; set; } = new();
    }
}