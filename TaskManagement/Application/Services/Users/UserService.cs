using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services.User;
using TaskManagement.Domain.Entities;
using TaskManagement.Shared.Helpers;
using TaskManagement.Shared.Responses;

namespace TaskManagement.Application.ServicesUser.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<object>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();

            return ResponseBuilder.Success<object>(
                users,
                "Users retrieved",
                true
            );
        }

        public async Task<Response<object>> CreateUserAsync(CreateUserDto model)
        {
            var exists = await _userManager.FindByEmailAsync(model.Email);

            if (exists != null)
                return ResponseBuilder.Failed<object>("Email already exists");

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return ResponseBuilder.Failed<object>(
                    string.Join(",", result.Errors.Select(x => x.Description))
                );

            await _userManager.AddToRoleAsync(user, model.Role);

            return ResponseBuilder.Success<object>(
                null,
                "User created",
                true
            );
        }

        public async Task<Response<object>> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return ResponseBuilder.Failed<object>("User not found");

            await _userManager.DeleteAsync(user);

            return ResponseBuilder.Success<object>(
                null,
                "User deleted",
                true
            );
        }
    }
}