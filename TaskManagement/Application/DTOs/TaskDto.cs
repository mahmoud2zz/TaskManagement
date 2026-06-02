using System;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs
{
	public class TaskDto
	{
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

