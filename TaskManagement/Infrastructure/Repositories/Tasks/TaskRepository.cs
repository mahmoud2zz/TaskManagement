using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Repositories.Tasks;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
            => await _context.TeskItems.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<TaskItem>> GetAllByUserAsync(string userId)
            => await _context.TeskItems.Where(x => x.UserId == userId).ToListAsync();

        public async Task AddAsync(TaskItem task)
            => await _context.TeskItems.AddAsync(task);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task<bool> AnyAsync(Expression<Func<TaskItem, bool>> predicate) =>  await _context.TeskItems.AnyAsync(predicate);

    }
}