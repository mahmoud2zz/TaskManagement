using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.DTOs;
using TaskManagement.Shared.Helpers;
using TaskManagement.Shared.Responses;
using TaskManagement.Domain.Entities;
namespace TaskManagement.Application.ServicesAuth
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<User> _userManager;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public AuthService(
            UserManager<User> userManager,
            JwtTokenHelper jwtTokenHelper)
        {
            _userManager = userManager;
            _jwtTokenHelper = jwtTokenHelper;
        }

        public async Task<Response<UserProfileDto>> GetCurrentUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return ResponseBuilder.Failed<UserProfileDto>("User not found");

            var result = new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email!,
                CreatedAt = user.CreatedAt
            };

            return ResponseBuilder.Success(result, "User profile retrieved successfully", true);
        }

        public async Task<Response<LoginResponseDto>> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return ResponseBuilder.Failed<LoginResponseDto>("Invalid email or password");

            var isValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isValidPassword)
                return ResponseBuilder.Failed<LoginResponseDto>("Invalid email or password");

            var jwtToken = await _jwtTokenHelper.CreateJwtToken(user, _userManager);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var response = new LoginResponseDto
            {
                Token = token,
                Email = user.Email!,
                Name = user.Name
            };

            return ResponseBuilder.Success(response, "Login successful", true);
        }
            public async Task<Response<RegisterResponseDto>> RegisterAsync(RegisterDto model)
        {
            var exists = await _userManager.FindByEmailAsync(model.Email);

            if (exists != null)
                return ResponseBuilder.Failed<RegisterResponseDto>("Email already exists");

            var user = new User
            {
                Name = model.Name,
                UserName = model.Email,
                Email = model.Email,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return ResponseBuilder.Failed<RegisterResponseDto>("User creation failed");

            await _userManager.AddToRoleAsync(user, "User");

            var response = new RegisterResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
            return ResponseBuilder.Success(response, "User registered successfully", true);
        }

          
    }
}

