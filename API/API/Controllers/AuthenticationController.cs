using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        //static user for authentication simulation
        public static User user = new User();

        private readonly IConfiguration configuration;
        private readonly IAuthenticationRepository authRepo;
        public AuthenticationController(IConfiguration configuration, IAuthenticationRepository authenticationRepository)
        {
            this.configuration = configuration;
            authRepo = authenticationRepository;
        }

        /// <summary>
        /// Method: POST
        /// Endpoint: api/authentication/register
        /// Description: Registers a new user and stores their information.
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserDTO userDto)
        {
            var user = await authRepo.RegisterAsync(userDto);
            if (user == null) 
                return BadRequest("Username already exists");

            return Ok(user);

        }

        /// <summary>
        /// Method: POST
        /// Endpoint: api/authentication/login
        /// Description: Authenticates a user and returns a JWT token if successful.
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDTO>> Login([FromBody] UserLoginDTO loginDto)
        {
            var result = await authRepo.LoginAsync(loginDto);
            if (result == null)
                return BadRequest("Invalid username or password");

            return Ok(result);
        }

        /// <summary>
        /// Method: POST
        /// Endpoint: api/authentication/refresh-token
        /// Description: Refreshes the JWT token using a valid refresh token.
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDTO>> RefreshToken([FromBody] RefreshTokenRequestDTO requestDto)
        {
            var result = await authRepo.RefreshTokensAsync(requestDto);
            if (result == null | result.RefreshToken == null | result.AccessToken == null)
                return BadRequest("Invalid refresh token");
            
            return Ok(result);
        }

        /// <summary>
        /// Method: GET
        /// Endpoint: api/authentication/authenticated-endpoint
        /// Description: An example endpoint that requires authentication.
        /// </summary>
        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedEndpoint()
        {
            return Ok("You are authenticated");
        }

        /// <summary>
        /// Method: GET
        /// Endpoint: api/authentication/admin-only
        /// Description: An example endpoint that requires admin role.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are Admin");
        }

    }
}
