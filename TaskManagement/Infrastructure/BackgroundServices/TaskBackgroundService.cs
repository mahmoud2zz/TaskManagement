using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Common.Queues;
using TaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Infrastructure.BackgroundServices
{
    public class TaskBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public TaskBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (TaskQueue.Tasks.Count > 0)
                {
                    var taskId = TaskQueue.Tasks.Dequeue();

                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var task = await db.TeskItems.FindAsync(taskId);

                    if (task != null)
                    {
                        task.Status = "InProgress";
                        await db.SaveChangesAsync();

                        await Task.Delay(3000);

                        task.Status = "Done";
                        await db.SaveChangesAsync();
                    }
                }

                await Task.Delay(5000);
            }
        }
    }
}