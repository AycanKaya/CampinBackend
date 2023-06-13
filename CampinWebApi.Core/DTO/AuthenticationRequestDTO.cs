using System;
using System.ComponentModel.DataAnnotations;

namespace CampinWebApi.Core.DTO
{
	public class AuthenticationRequestDTO
	{
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}

