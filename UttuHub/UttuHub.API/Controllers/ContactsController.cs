using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UttuHub.API.Data;
using UttuHub.API.DTOs.Contact;   // ADDED: DTO namespace
using UttuHub.API.Models;

namespace UttuHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactsController(AppDbContext context)
        {
            _context = context;
        }

        // UC 241 - Create Contact
        // CHANGED: Now accepts ContactCreateDto - Id, IsApproved, SentAt, UserId all set server-side
        // CHANGED: UserId resolved from username in route - visitor never sends it
        [HttpPost("~/api/users/{username}/contacts")]
        public async Task<ActionResult<Contact>> PostContact(string username, ContactCreateDto dto)
        {
            // ADDED: Look up the admin by username to get their UserId automatically
            var admin = await _context.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == username.ToLower());
            if (admin == null) return NotFound($"No portfolio found for '{username}'.");

            // Map DTO to Contact model
            var contact = new Contact
            {
                Name = dto.Name,
                Email = dto.Email,
                Message = dto.Message,
                Address = dto.Address, // Optional field
                IsPublic = dto.IsPublic,
                IsApproved = false,         // Default to false so you can curate them
                SentAt = DateTime.UtcNow,   // ADDED: Set server-side, not from client
                UserId = admin.Id           // ADDED: Set automatically from route, visitor never sends this
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            // FIXED: was pointing to GetContacts (list), should point to GetContact (single)
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        // UC 241.1 - GET single Contact by ID ← ADDED
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactResponseDto>> GetContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null) return NotFound();

            // Map to DTO before returning
            var result = new ContactResponseDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                Message = contact.Message,
                Address = contact.Address, // Optional field
                IsPublic = contact.IsPublic,
                IsApproved = contact.IsApproved,
                SentAt = contact.SentAt,
                UserId = contact.UserId
            };

            return Ok(result);
        }

        // UC 242- GET all Contact
        // CHANGED: Now returns List<ContactResponseDto> instead of raw Contact list
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactResponseDto>>> GetContacts()
        {
            // Usually, you'll want to see the latest messages first
            return await _context.Contacts
                .OrderByDescending(c => c.SentAt)
                .Select(c => new ContactResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Message = c.Message,
                    Address = c.Address, // Optional field
                    IsPublic = c.IsPublic,
                    IsApproved = c.IsApproved,
                    SentAt = c.SentAt,
                    UserId = c.UserId
                })
                .ToListAsync();
        }

        // UC 242 - Update Contact to Approved
        // CHANGED: Blocks approval if IsPublic is false
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            // ADDED: Cannot approve a contact that is not public
            if (!contact.IsPublic)
            {
                return BadRequest("Cannot approve a contact that is not public.");
            }

            // FIXED: was setting to false (bug in original), should be true to actually approve
            contact.IsApproved = true;
            _context.Entry(contact).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // UC 243 - Delete Contact
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}


