using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UttuHub.API.Data;
using UttuHub.API.DTOs.User;   // ADDED: DTO namespace
using UttuHub.API.Models;

namespace UttuHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // UC 211 - Create User (Register)
        // CHANGED: Now accepts RegisterDto instead of raw User model
        // CHANGED: Added ImageUrl to RegisterDto
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest("Email already exists.");
            }

            // Map DTO to User model
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                ImageUrl = dto.ImageUrl,
                isVerified = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully! Awaiting verification." });
        }

        // 2. UC - 212 - login
        // CHANGED: Now accepts LoginDto instead of raw User model
        // CHANGED: Returns token only (removed user name from response)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null) return Unauthorized("Invalid Email.");

            if (!user.isVerified) return Unauthorized("Account not verified by Admin.");

            bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isValid) return Unauthorized("Invalid Password.");

            var token = GenerateJwtToken(user);
            return Ok(new { token = token }); // CHANGED: Returns token only
        }

        // UC 213 - Verify
        [HttpPut("verify/{id}")]
        public async Task<IActionResult> VerifyUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found.");

            user.isVerified = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"User {user.Name} has been verified successfully." });
        }

        // UC 214 - Get Single User Profile
        // CHANGED: Now returns UserResponseDto instead of anonymous object
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null) return NotFound("User not found.");

            var result = new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ImageUrl = user.ImageUrl,
                IsVerified = user.isVerified
            };

            return Ok(result);
        }

        // UC 215 - Get all Users 
        // CHANGED: Now returns List<UserResponseDto> instead of anonymous object list
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            return await _context.Users
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    ImageUrl = u.ImageUrl,
                    IsVerified = u.isVerified
                })
                .ToListAsync();
        }

        // UC 216 - Update User ← ADDED
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserUpdateDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found.");

            // Update fields - isVerified intentionally NOT updatable here (use verify endpoint)
            user.Name = dto.Name;
            user.Email = dto.Email;
            user.ImageUrl = dto.ImageUrl;

            // Only re-hash and update password if a new one was provided
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // UC 217 - Delete User ← ADDED
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // JWT Generator
        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "A_Very_Long_BackUp_Secret_Key_32_Chars"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}