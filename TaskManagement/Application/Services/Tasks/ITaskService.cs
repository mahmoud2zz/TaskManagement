using System;
using TaskManagement.Application.DTOs;
using TaskManagement.Shared.Responses;

namespace TaskManagement.Application.Services.Tasks
{
	public interface ITaskService
	{
        Task<Response<TaskDto>> CreateAsync(CreateTaskDto dto, string userId);
        Task<Response<List<TaskDto>>> GetMyTasksAsync(string userId);
        Task<Response<TaskDto>> GetByIdAsync(int id, string userId);
        Task<Response<bool>> UpdateStatusAsync(int id, string status, string userId);
    }
}

