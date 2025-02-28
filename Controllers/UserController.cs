using System.Runtime.InteropServices.ObjectiveC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.DTOs;

namespace TodoApi.Controllers
{
    [Route("api/users")]  // Route for User APIs
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TodoDbContext _context;

        public UserController(TodoDbContext context)
        {
            _context = context;
        }

        // Get all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // Get a user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // Create a new user
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // Update a user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id) return BadRequest();
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Delete a user (soft delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        //Sign up API
        [HttpPost("signup")]
        public async Task<ActionResult<User>> SignUp(UserRegisterDto.UsersRegisterDto userDto)
        {
            //Check if the email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already exists.");
            }
            
            //Create a new user object
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = Models.User.HashPassword(userDto.Password),//Hash password 
                Role = "User"
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        
        //User login API
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || user.PasswordHash != Models.User.HashPassword(loginDto.Password))
            {
                return Unauthorized(new { message = "Username or password is incorrect." });
            }
            
            return Ok(new
            {
                message = "Login successful.",
                userId = user.Id,
                username = user.Username,
                email = user.Email,
                role = "User"
            });
        }
    }
        
}

