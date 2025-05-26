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
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /* Deleted - we have register
            /// <summary>
            /// Method: Post
            /// Endpoint: `/api/users`
            /// Description: Create a new user (Admin only).
            /// </summary>
            [HttpPost]
            [Authorize(Roles = "Admin")]
            public async Task<ActionResult<User>> CreateUser([FromBody] UserDTO userDTO)
            {
                var user = await _authRepo.RegisterAsync(userDTO);

                if (user == null)
                    return BadRequest("Username already exists");


                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
         */

        /// <summary>
        /// Method: Put
        /// Endpoint: `/api/users/{id}`
        /// Description: Update user details by ID (Admin or User).
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> UpdateUser(Guid id, [FromBody] UserDTO updatedUserDTO)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = updatedUserDTO.FullName;
            user.Username = updatedUserDTO.Username;
            user.Email = updatedUserDTO.Email;
            user.Phone = updatedUserDTO.Phone;
            user.Role = updatedUserDTO.Role;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Method: Delete
        /// Endpoint: `/api/users/{id}`
        /// Description: Delete a user by ID (Admin or User).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
    }
   
}
