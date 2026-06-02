using System;
using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<TaskItem> TaskItems { get; set; } = new();

    }
}

