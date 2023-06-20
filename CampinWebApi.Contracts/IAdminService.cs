using Microsoft.AspNetCore.Identity;

namespace CampinWebApi.Contracts;

public interface IAdminService
{
    Task<bool> DeleteCampsite(string id);
    Task<bool> ChangeUserRole(string userId, string newRole);
    Task<bool> DeleteUser(string id);
    Task<bool> DeleteComment(int id);
    Task<bool> DeleteReservation(int rezervationId);
    Task<IdentityUser[]> GetAllUsers();
}