using System;
using System.Linq.Expressions;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Repositories.Tasks
{
	public interface ITaskRepository
	{
        Task<TaskItem?> GetByIdAsync(int id);
        Task<List<TaskItem>> GetAllByUserAsync(string userId);
        Task AddAsync(TaskItem task);
        Task<bool> AnyAsync(Expression<Func<TaskItem, bool>> predicate);
        Task SaveChangesAsync();
    }
}

