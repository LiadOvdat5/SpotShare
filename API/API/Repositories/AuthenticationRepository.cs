using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly SpotShareDBContext _spotShareDBContext;
        private readonly IConfiguration _configuration;

        public AuthenticationRepository(SpotShareDBContext spotShareDBContext, IConfiguration configuration)
        {
            _spotShareDBContext = spotShareDBContext;
            _configuration = configuration;

        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        public async Task<User?> RegisterAsync(UserDTO userDto)
        {
            // if user aleady exists
            if (await _spotShareDBContext.Users.AnyAsync(u => u.Username == userDto.Username))
                return null;

            var user = new User();

            user.FullName = userDto.FullName;
            user.Email = userDto.Email;
            user.Username = userDto.Username;
            user.Phone = userDto.Phone;
            user.Role = userDto.Role;
            user.DateCreated = DateTime.UtcNow;


            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(new User(), userDto.Password);

            user.PasswordHash = hashedPassword;

            _spotShareDBContext.Add(user);
            await _spotShareDBContext.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>

        public async Task<TokenResponseDTO?> LoginAsync(UserLoginDTO loginDto)
        {
            var user = await _spotShareDBContext.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null)
                return null;

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginDto.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        /// <summary>
        /// Generates a JWT token for the authenticated user.
        /// </summary>
        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken
            (
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        /// <summary>
        /// Generates a refresh token for the user.
        /// </summary>
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        /// <summary>
        /// Generates a refresh token for the user and saves it in the database.
        /// </summary>
        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Set expiry time for 7 days
            await _spotShareDBContext.SaveChangesAsync();
            return refreshToken;
        }

        /// <summary>
        /// Validates the refresh token for the user.
        /// </summary>
        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await _spotShareDBContext.Users.FindAsync(userId);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow || user.RefreshToken != refreshToken)
            {
                return null; // Invalid or expired refresh token
            }
            return user;
        }

        /// <summary>
        /// Refreshes the access token and refresh token for the user using the provided refresh token.
        /// </summary>
        public async Task<TokenResponseDTO?> RefreshTokensAsync(RefreshTokenRequestDTO requestDTO)
        {
            var user = await ValidateRefreshTokenAsync(requestDTO.UserId, requestDTO.RefreshToken);
            if (user == null)
            {
                return null; // Invalid or expired refresh token
            }

            return await CreateTokenResponse(user);
        }

        /// <summary>
        /// Creates a token response containing the access token and refresh token for the user.
        /// </summary>
        private async Task<TokenResponseDTO> CreateTokenResponse(User? user)
        {
            var response = new TokenResponseDTO
            {
                AccessToken = GenerateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };

            return response;
        }
    }
}
