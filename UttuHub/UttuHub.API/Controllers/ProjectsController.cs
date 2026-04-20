using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UttuHub.API.Data;
using UttuHub.API.DTOs.Project;   // ADDED: DTO namespace
using UttuHub.API.Models;

namespace UttuHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        // UC 231 - Create Project
        // CHANGED: Now accepts ProjectCreateDto instead of raw Project model
        // CHANGED: UserId now extracted from JWT claims instead of being sent in request body
        [HttpPost]
        public async Task<ActionResult<ProjectResponseDto>> PostProject(ProjectCreateDto dto)
        {
            // ADDED: Extract UserId from JWT token claims (logged in user only)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized("User not identified.");
            var userId = int.Parse(userIdClaim);

            // Map DTO to Project model
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                TechStack = dto.TechStack,
                GithubUrl = dto.GithubUrl,
                LiveUrl = dto.LiveUrl,
                UserId = userId // CHANGED: Set from JWT, not from request body
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var result = new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                TechStack = project.TechStack,
                GithubUrl = project.GithubUrl,
                LiveUrl = project.LiveUrl,
                UserId = project.UserId
            };

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, result); // ✅ Fixed: was nameof(GetProjects)
        }

        // UC 231.1 - Get single Project by ID (required for CreatedAtAction)
        // CHANGED: Now returns ProjectResponseDto instead of raw Project model
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponseDto>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            var result = new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                TechStack = project.TechStack,
                GithubUrl = project.GithubUrl,
                LiveUrl = project.LiveUrl,
                UserId = project.UserId
            };

            return Ok(result);
        }

        // UC 232 - GET all Projects
        // CHANGED: Now returns List<ProjectResponseDto> instead of raw Project list
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetProjects()
        {
            return await _context.Projects
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    TechStack = p.TechStack,
                    GithubUrl = p.GithubUrl,
                    LiveUrl = p.LiveUrl,
                    UserId = p.UserId
                })
                .ToListAsync();
        }

        // UC 233 - Update Project
        // CHANGED: Now accepts ProjectUpdateDto instead of raw Project model
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, ProjectUpdateDto dto)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            // Update fields - UserId intentionally NOT updatable (ownership cannot be transferred)
            project.Name = dto.Name;
            project.Description = dto.Description;
            project.TechStack = dto.TechStack;
            project.GithubUrl = dto.GithubUrl;
            project.LiveUrl = dto.LiveUrl;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Projects.Any(e => e.Id == id))
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

        // UC 234 - Delete Project
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}