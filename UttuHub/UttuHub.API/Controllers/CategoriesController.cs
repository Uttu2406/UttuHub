using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UttuHub.API.Data;
using UttuHub.API.Models;

namespace UttuHub.API.Controllers
{
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
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostCategory), new { id = category.Id }, category); // only requires name.
        }

        // UC 202 - GET all Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync(); // fetch and store in a list.
        }

        // UC 203 - Update Category
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest("ID mismatch");
            }

            _context.Entry(category).State = EntityState.Modified; // naya cat haina purano ma modify gareko vanera

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
