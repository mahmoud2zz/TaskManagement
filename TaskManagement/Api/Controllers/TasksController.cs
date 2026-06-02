using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services.Tasks;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize] // ✔ كل الـ endpoints محمية
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service)
        {
            _service = service;
        }

        // ➕ Create Task
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {

            var userId = User.FindFirst("uid")?.Value;

            var result = await _service.CreateAsync(dto, userId!);

            return Ok(result);
        }

        // 📋 Get My Tasks
        [HttpGet]
        public async Task<IActionResult> GetMyTasks()
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = await _service.GetMyTasksAsync(userId!);

            return Ok(result);
        }

        // 🔍 Get Task By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = await _service.GetByIdAsync(id, userId!);

            return Ok(result);
        }

        // 🔄 Update Status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTaskStatusDto dto)
        {
            var userId = User.FindFirst("uid")?.Value;
            var result = await _service.UpdateStatusAsync(id, dto.Status, userId!);

            return Ok(result);
        }
    }
}