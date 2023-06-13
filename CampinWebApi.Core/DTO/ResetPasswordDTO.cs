using System.ComponentModel.DataAnnotations;

namespace CampinWebApi.Core.DTO;

public class ResetPasswordDTO
{
        public string Email { get; set; }

        public string OldPassword { get; set; }
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

}