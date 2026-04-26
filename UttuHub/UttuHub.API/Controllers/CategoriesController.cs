using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UttuHub.API.Data;
using UttuHub.API.DTOs.Category;   // ADDED: DTO namespace
using UttuHub.API.Models;

namespace UttuHub.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // UC 201 - Create Category
        // CHANGED: Now accepts CategoryCreateDto instead of raw Category model (removes Id from request)
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CategoryCreateDto dto)
        {
            // Map DTO to Category model
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                IconKey = dto.IconKey,
                HexColor = dto.HexColor
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // FIXED: was pointing to PostCategory (wrong), should point to GetCategory (single)
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category); // only requires name.
        }

        // UC 201.1 - GET single Category by ID ← ADDED
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null) return NotFound();

            return Ok(category);
        }

        // UC 202 - GET all Category
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync(); // Simple list fetch - JsonIgnore in the model handles the rest
        }

        // UC 203 - Update Category
        // CHANGED: Now accepts CategoryUpdateDto instead of raw Category model (removes Id from request body)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryUpdateDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            // Update fields - Id taken from route, not request body
            category.Name = dto.Name;
            category.Description = dto.Description;
            category.IconKey = dto.IconKey;
            category.HexColor = dto.HexColor;

            // Check for data sync issues
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(e => e.Id == id))
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

        // UC 204 - Delete Category
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            try
            {
                _context.Categories.Remove(category); // Remove cat
                await _context.SaveChangesAsync(); // Update removal in db
            }
            catch (DbUpdateException)
            {
                return BadRequest("Cannot delete this category because it is currently linked to feed items or projects.");
            }

            return NoContent();
        }
    }
}