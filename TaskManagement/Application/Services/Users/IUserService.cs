using System;
using TaskManagement.Application.DTOs;
using TaskManagement.Shared.Responses;

namespace TaskManagement.Application.Services.User
{
	public interface IUserService
	{
        Task<Response<object>> GetAllUsersAsync();
        Task<Response<object>> CreateUserAsync(CreateUserDto model);
        Task<Response<object>> DeleteUserAsync(string id);
    }
}

