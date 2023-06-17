using System;
using System.ComponentModel.DataAnnotations;

namespace CampinWebApi.Core.DTO
{
	public class RegisterRequestDTO
	{

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        
        [Required(ErrorMessage = "Surname  is required")]
        public string? Surname { get; set; }
        
         
        [Required(ErrorMessage = "PhoneName  is required")] 
        public string? PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        
        

	}
}

