
using TaskManagement.Application.DTOs;
using TaskManagement.Shared.Responses;

namespace TaskManagement.Application.ServicesAuth
{
	public interface IAuthService
	{
        Task<Response<RegisterResponseDto>> RegisterAsync(RegisterDto model);

        Task<Response<LoginResponseDto>> LoginAsync(LoginDto model);
        Task<Response<UserProfileDto>> GetCurrentUserAsync(string userId);
    }
}

