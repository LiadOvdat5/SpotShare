using API.Models;
using API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Repositories;
using API.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {


        private readonly SpotShareDBContext _context;
        private readonly IAuthenticationRepository _authRepo;
        public UsersController(SpotShareDBContext context, IAuthenticationRepository authenticationRepository)
        {
            _context = context;
            _authRepo = authenticationRepository;
        }

        /// <summary>
        /// Method: Get
        /// Endpoint: `/api/users`
        /// Description: Get all users (Admin only).
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        /// <summary>
        /// Method: Get
        /// Endpoint: `/api/users/{id}`
        /// Description: Get user details by ID (Admin or User).
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            // Check if the user is requesting their own details
            if (User.IsInRole("User") && User.FindFirst(ClaimTypes.NameIdentifier)?.Value != id.ToString())
            {
                return Forbid(); // User can only access their own details
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Method: Put
        /// Endpoint: `/api/users/{id}`
        /// Description: Update user details by ID (Admin or User).
        /// </summary>
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> UpdateUser([FromBody] UserUpdateDTO updatedUserDTO)
        {
            var stringId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (stringId == null)
                return Unauthorized();

            var id = Guid.Parse(stringId);

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = updatedUserDTO.FullName ?? user.FullName;
            user.Username = updatedUserDTO.Username ?? user.Username;
            user.Email = updatedUserDTO.Email ?? user.Email;
            user.Phone = updatedUserDTO.Phone ?? user.Phone;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Method: Delete
        /// Endpoint: `/api/users/{id}`
        /// Description: Delete a user by ID (Admin or User).
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Method: Put
        /// Endpoint: `/api/users/{id}/password`
        /// Description: Update user password by ID (Admin or User).
        /// </summary>
        [HttpPut("{id}/password")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> UpdatePassword([FromBody] UpdatePasswordDTO updatePasswordDTO)
        {
            var stringId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (stringId == null)
                return Unauthorized();
            var userId = Guid.Parse(stringId);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound();

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, updatePasswordDTO.CurrentPassword)
                == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Password provided is wrong.");
            }

            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, updatePasswordDTO.NewPassword);

            user.PasswordHash = hashedPassword;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();

        }



    }

}
