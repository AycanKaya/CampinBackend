using CampinWebApi.Contracts;
using CampinWebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CampinWebApi.Services;

public class AdminService :IAdminService
{
    private readonly CampinDbContext context;
    UserManager<IdentityUser> userManager;
    
    public AdminService(CampinDbContext context,
        UserManager<IdentityUser> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }
    
    public async Task<IdentityUser[]> GetAllUsers()
    {
        var UserList = await userManager.Users.ToArrayAsync();
        if (UserList == null)
            throw new ArgumentNullException(nameof(UserList));
        return UserList;
    }
    
    // Admin can delete the campsites
    public async Task<bool> DeleteCampsite(string id)
    {
        var campsite = await context.Campsites.FindAsync(id);
        
        // campsite'a bağlı rezervasoynları sil 
        var reservations = await context.Rezervations.Where(x => x.CampsiteId == id).ToListAsync();
        context.Rezervations.RemoveRange(reservations);
        
        //camsite'a bağlı yorumları sil
        var comments = await context.Comments.Where(x => x.CampsiteId == id).ToListAsync();
        context.Comments.RemoveRange(comments);

        if (campsite == null)
            throw new FileNotFoundException("Campsite not found");

        context.Campsites.Remove(campsite);
        await context.SaveChangesAsync();
        return true;
    }
    
    // Admin can change user role 
    public async Task<bool> ChangeUserRole(string userId, string newRole)
    {
        var existInfo = await context.UserInfo.Where(x => x.UserID == userId).FirstOrDefaultAsync();

        var user = await userManager.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        if (user == null)
            throw new ArgumentNullException();

        var role = await userManager.GetRolesAsync(user);
        if(role != null)
            await userManager.RemoveFromRoleAsync(user, role.ToString());

        await userManager.AddToRoleAsync(user, newRole);
        existInfo.Role = newRole;
        await context.SaveChangesAsync();
        return true;
    }
    
    // Admin can delete the user
    public async Task<bool> DeleteUser(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
            throw new FileNotFoundException("User not found");

        var userInfo = await context.UserInfo.Where(x => x.UserID == id).FirstOrDefaultAsync();
        context.UserInfo.Remove(userInfo);
        await context.SaveChangesAsync();

        await userManager.DeleteAsync(user);
        return true;
    }
}

