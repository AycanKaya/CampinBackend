using CampinWebApi.Domain.Enums;

namespace CampinWebApi.Core.DTO.UserDTO;

public class EditUserInfoDTO
{
    public string UserID { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public Gender? Gender { get; set; }
    public string? Contry { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Role { get; set; }
    public string? Email { get; set; }
    
}