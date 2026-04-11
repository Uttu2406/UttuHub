using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UttuHub.API.Data;
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

            return CreatedAtAction(nameof(GetContacts), new { id = contact.Id }, contact);
        }

        // UC 242- GET all Contact
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            // Usually, you'll want to see the latest messages first
            return await _context.Contacts
                .OrderByDescending(c => c.SentAt)
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