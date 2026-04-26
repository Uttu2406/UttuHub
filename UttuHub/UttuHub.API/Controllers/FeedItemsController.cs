using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UttuHub.API.Data;
using UttuHub.API.DTOs.Category;   // ADDED: CategorySummaryDto
using UttuHub.API.DTOs.FeedItem;   // ADDED: FeedItem DTOs
using UttuHub.API.Models;

namespace UttuHub.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FeedItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public FeedItemsController(AppDbContext context) { _context = context; }

        // UC 221 - Create FeedItem with Multiple Categories
        // CHANGED: Now accepts FeedItemCreateDto instead of inline DTO defined at bottom of file
        // CHANGED: UserId now extracted from JWT claims instead of being sent in request body
        [HttpPost]
        public async Task<ActionResult> PostFeedItem(FeedItemCreateDto dto)
        {
            // ADDED: Extract UserId from JWT token claims (logged in user only)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized("User not identified.");
            var userId = int.Parse(userIdClaim);

            var feedItem = new FeedItem
            {
                Title = dto.Title,
                Content = dto.Content,
                ImageUrl = dto.ImageUrl,
                IsHighlight = dto.IsHighlight,
                UserId = userId, // CHANGED: Set from JWT, not from request body
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

            return CreatedAtAction(nameof(GetFeedItem), new { id = feedItem.Id }, result); // ✅ Fixed: was nameof(PostFeedItem)
        }

        // UC 221.1 - Get single FeedItem by ID (required for CreatedAtAction)
        // CHANGED: Now returns FeedItemResponseDto instead of anonymous object
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedItemResponseDto>> GetFeedItem(int id)
        {
            var f = await _context.FeedItems
                .Include(f => f.FeedItemCategories)
                    .ThenInclude(fc => fc.Category)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (f == null) return NotFound();

            var result = new FeedItemResponseDto
            {
                Id = f.Id,
                Title = f.Title,
                Content = f.Content,
                ImageUrl = f.ImageUrl,
                Created = f.Created,
                IsHighlight = f.IsHighlight,
                UserId = f.UserId,
                Categories = f.FeedItemCategories.Select(fc => new CategorySummaryDto
                {
                    Id = fc.Category!.Id,
                    Name = fc.Category.Name,
                    HexColor = fc.Category.HexColor,
                    IconKey = fc.Category.IconKey
                }).ToList()
            };

            return Ok(result);
        }

        // UC 222 - Read all FeedItems (Including Categories)
        // CHANGED: Now returns List<FeedItemResponseDto> instead of anonymous object list
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedItemResponseDto>>> GetFeedItems()
        {
            var feedItems = await _context.FeedItems
                .Include(f => f.FeedItemCategories)
                    .ThenInclude(fc => fc.Category)
                .OrderByDescending(f => f.Created)
                .ToListAsync();

            var result = feedItems.Select(f => new FeedItemResponseDto
            {
                Id = f.Id,
                Title = f.Title,
                Content = f.Content,
                ImageUrl = f.ImageUrl,
                Created = f.Created,
                IsHighlight = f.IsHighlight,
                UserId = f.UserId,
                Categories = f.FeedItemCategories.Select(fc => new CategorySummaryDto
                {
                    Id = fc.Category!.Id,
                    Name = fc.Category.Name,
                    HexColor = fc.Category.HexColor,
                    IconKey = fc.Category.IconKey
                }).ToList()
            });

            return Ok(result);
        }

        // UC 223 - Update FeedItem
        // CHANGED: Now accepts FeedItemUpdateDto instead of raw FeedItem model
        // CHANGED: Category links are fully replaced (cleared and re-inserted) on every update
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedItem(int id, FeedItemUpdateDto dto)
        {
            var feedItem = await _context.FeedItems
                .Include(f => f.FeedItemCategories)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (feedItem == null) return NotFound();

            // Update simple fields
            feedItem.Title = dto.Title;
            feedItem.Content = dto.Content;
            feedItem.ImageUrl = dto.ImageUrl;
            feedItem.IsHighlight = dto.IsHighlight;
            // Created and UserId are intentionally NOT updated

            // Replace all category links (delete old, insert new)
            feedItem.FeedItemCategories.Clear();
            if (dto.CategoryIds != null && dto.CategoryIds.Any())
            {
                foreach (var catId in dto.CategoryIds)
                {
                    feedItem.FeedItemCategories.Add(new FeedItemCategory { FeedItemId = id, CategoryId = catId });
                }
            }

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


}


