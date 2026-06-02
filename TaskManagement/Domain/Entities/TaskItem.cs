using System;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
	public class TaskItem
	{
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = "Pending"; // Pending | InProgress | Done
        public TaskPriority Priority { get; set; } = TaskPriority.Low;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}

