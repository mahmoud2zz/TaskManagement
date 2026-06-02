using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TaskManagement.Application.Common.Queues;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Repositories.Tasks;
using TaskManagement.Shared.Helpers;
using TaskManagement.Shared.Responses;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repo;
    private readonly IDistributedCache _cache;
    private readonly ILogger<TaskService> _logger;

public TaskService(
    ITaskRepository repo,
    IDistributedCache cache,
    ILogger<TaskService> logger)
    {
        _repo = repo;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Response<TaskDto>> CreateAsync(CreateTaskDto dto, string userId)
    {
        var today = DateTime.UtcNow.Date;

        var exists = await _repo.AnyAsync(t =>
            t.UserId == userId &&
            t.Title == dto.Title &&
            t.CreatedAt.Date == today);

        if (exists)
        {
            _logger.LogWarning(
                "Duplicate task creation attempt. UserId={UserId}, Title={Title}",
                userId,
                dto.Title);

            return ResponseBuilder.Failed<TaskDto>(
                "Task already exists today with same title");
        }

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            Status = "Pending",
            UserId = userId
        };

        await _repo.AddAsync(task);
        await _repo.SaveChangesAsync();

        TaskQueue.Tasks.Enqueue(task.Id);

        _logger.LogInformation(
            "Task created successfully. TaskId={TaskId}, UserId={UserId}",
            task.Id,
            userId);

        return ResponseBuilder.Success(
            new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                Status = task.Status,
                CreatedAt = task.CreatedAt
            },
            "Task created",
            true);
    }

    public async Task<Response<List<TaskDto>>> GetMyTasksAsync(string userId)
    {
        _logger.LogInformation(
            "Retrieving tasks for UserId={UserId}",
            userId);

        var tasks = await _repo.GetAllByUserAsync(userId);

        var sorted = tasks
            .OrderByDescending(t => t.Priority)
            .ThenByDescending(t => t.CreatedAt)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Priority = t.Priority,
                Status = t.Status,
                CreatedAt = t.CreatedAt
            })
            .ToList();

        return ResponseBuilder.Success(
            sorted,
            "Tasks retrieved",
            true);
    }

    public async Task<Response<TaskDto>> GetByIdAsync(int id, string userId)
    {



        var cacheKey = $"task:{id}";

        var cachedTask = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedTask))
        {
            _logger.LogInformation(
                "Task {TaskId} returned from Redis cache",
                id);

            var cachedDto =
                JsonSerializer.Deserialize<TaskDto>(cachedTask);

            if (cachedDto != null)
                return ResponseBuilder.Success(
                    cachedDto,
                    "From Cache",
                    true);
        }

        _logger.LogInformation(
            "Task {TaskId} not found in cache. Loading from database",
            id);

        var task = await _repo.GetByIdAsync(id);

        if (task == null || task.UserId != userId)
        {
            _logger.LogWarning(
                "Task access denied or not found. TaskId={TaskId}, UserId={UserId}",
                id,
                userId);

            return ResponseBuilder.Failed<TaskDto>("Not found");
        }

        var dto = new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            Status = task.Status,
            CreatedAt = task.CreatedAt
        };

        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(dto),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes(5)
            });

        _logger.LogInformation(
            "Task {TaskId} stored in Redis cache",
            id);

        return ResponseBuilder.Success(
            dto,
            "From DB",
            true);
    }

    public async Task<Response<bool>> UpdateStatusAsync(
        int id,
        string status,
        string userId)
    {
        var task = await _repo.GetByIdAsync(id);

        if (task == null || task.UserId != userId)
        {
            _logger.LogWarning(
                "Task update denied. TaskId={TaskId}, UserId={UserId}",
                id,
                userId);

            return ResponseBuilder.Failed<bool>("Not allowed");
        }

        var oldStatus = task.Status;

        task.Status = status;

        await _repo.SaveChangesAsync();

        _logger.LogInformation(
            "Task {TaskId} status changed from {OldStatus} to {NewStatus}",
            id,
            oldStatus,
            status);

        await _cache.RemoveAsync($"task:{id}");

        _logger.LogInformation(
            "Redis cache invalidated for TaskId={TaskId}",
            id);

        return ResponseBuilder.Success(
            true,
            "Updated",
            true);
    }

}
