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
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            contact.SentAt = DateTime.UtcNow;
            contact.IsApproved = false; // Default to false so you can curate them

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            // FIXED: was pointing to GetContacts (list), should point to GetContact (single)
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        // UC 241.1 - GET single Contact by ID  ← ADDED
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
                Address = contact.Address,
                IsPublic = contact.IsPublic,
                IsApproved = contact.IsApproved,
                SentAt = contact.SentAt
            };

            return Ok(result);
        }

        // UC 242 - GET all Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactResponseDto>>> GetContacts()
        {
            // Usually, you'll want to see the latest messages first
            // CHANGED: Now returns ContactResponseDto instead of raw Contact model
            return await _context.Contacts
                .OrderByDescending(c => c.SentAt)
                .Select(c => new ContactResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Message = c.Message,
                    Address = c.Address,
                    IsPublic = c.IsPublic,
                    IsApproved = c.IsApproved,
                    SentAt = c.SentAt
                })
                .ToListAsync();
        }

        // UC 242 - Update Contact to Approved
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
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