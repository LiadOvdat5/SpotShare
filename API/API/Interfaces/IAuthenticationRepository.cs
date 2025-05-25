using API.DTOs;
using API.Models;

namespace API.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<User?> RegisterAsync(UserDTO userDTO);
        Task<TokenResponseDTO?> LoginAsync(UserLoginDTO loginDTO);

        Task<TokenResponseDTO?> RefreshTokensAsync(RefreshTokenRequestDTO requestDTO);
    }
}
