using System;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs
{
	public class CreateTaskDto
	{
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TaskPriority Priority { get; set; }
    }
}

