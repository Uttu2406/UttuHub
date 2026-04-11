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
        public FeedItemsController(AppDbContext context)
        {
            _context = context;
        }

        // UC 221 - Create Category
        [HttpPost]
        public async Task<ActionResult<FeedItem>> PostFeedItem(FeedItem feedItem)
        {
            feedItem.Created = DateTime.UtcNow; 
            _context.FeedItems.Add(feedItem); // Updates this to database
            await _context.SaveChangesAsync();
    
            return CreatedAtAction(nameof(PostFeedItem), new { id = feedItem.Id }, feedItem); 
        }

        // UC 222 - Read all FeedItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedItem>>> GetFeedItems()
        {
            return await _context.FeedItems
                .Include(f => f.Category) // Tells EF to join table Category
                .OrderByDescending(f => f.Created) // Show newest first
                .ToListAsync(); // Convert into list and store
        }

        // UC 223 - Update FeedItem
        [HttpPut("{id}")]
        public async Task<ActionResult> PutFeedItem(int id, FeedItem feedItem)
        {
            if(id != feedItem.Id)
            {
                return BadRequest("ID mismatch");
            }

            _context.Entry(feedItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!_context.FeedItems.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // UC 224 - Delete FeedItem
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedItem(int id)
        {
            var feedItem = await _context.FeedItems.FindAsync(id);

            if (feedItem == null)
            {
                return NotFound();
            }

            _context.FeedItems.Remove(feedItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

