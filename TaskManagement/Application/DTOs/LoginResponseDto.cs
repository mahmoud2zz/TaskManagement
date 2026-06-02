using System;
namespace TaskManagement.Application.DTOs
{
	public class LoginResponseDto
	{
        public string Token { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Name { get; set; } = null!;

    }
}

