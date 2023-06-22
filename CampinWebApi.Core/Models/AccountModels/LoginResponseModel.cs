using System.Text.Json.Serialization;
using CampinWebApi.Domain.Enums;

namespace CampinWebApi.Core.Models.AccountModels;

public class LoginResponseModel
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
    public string JWToken { get; set; }
    [JsonIgnore]
    public string RefreshToken { get; set; }
    
    public string[] FavoritedCampsiteIds { get; set; }
}