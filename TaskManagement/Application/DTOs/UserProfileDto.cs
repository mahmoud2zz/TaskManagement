using System;
namespace TaskManagement.Application.DTOs
{
	public class UserProfileDto
	{

        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}

