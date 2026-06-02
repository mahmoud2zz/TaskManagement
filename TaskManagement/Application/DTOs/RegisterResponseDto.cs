using System;
namespace TaskManagement.Application.DTOs
{
	public class RegisterResponseDto
	{
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}

