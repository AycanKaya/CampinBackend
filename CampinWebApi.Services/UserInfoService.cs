using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.UserDTO;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampinWebApi.Services;

public class UserInfoService :IUserInfoService
{
    private CampinDbContext context;
    private IJWTService jwtService;
    public UserInfoService(CampinDbContext context, IJWTService jwtService)
    {
        this.context = context;
        this.jwtService = jwtService;
    }
    
    public async Task<UserInfo> GetUserInfo(string token)
    {
        var userId = jwtService.GetUserIdFromJWT(token);
        return context.UserInfo.Find(userId);
    }

    public async Task<UserInfo> EditUserInfo(EditUserInfoDTO dto, string userToken)
    {
        var userId = jwtService.GetUserIdFromJWT(userToken);
        var userInfo = await context.UserInfo.FirstOrDefaultAsync(x => x.UserID == userId);
        
        if(userInfo == null)
           return await AddUserInfo(dto, userId);
        
        return await EditUserInfo(dto, userInfo);
    }

    private async Task<UserInfo> AddUserInfo(EditUserInfoDTO dto , string userId)
    {
        var userInfo = new UserInfo();
        userInfo.UserID = userId;
        userInfo.Name = dto.Name;
        userInfo.Surname = dto.Surname;
        userInfo.Address = dto.Address;
        userInfo.Contry = dto.Contry;
        userInfo.Email = dto.Email;
        userInfo.PhoneNumber = dto.PhoneNumber;
        userInfo.Role = dto.Role;
        
        await context.UserInfo.AddAsync(userInfo);
        await context.SaveChangesAsync();
        return userInfo;
    }
    
    private async Task<UserInfo> EditUserInfo(EditUserInfoDTO dto , UserInfo userInfo)
    {
        userInfo.Name = dto.Name;
        userInfo.Surname = dto.Surname;
        userInfo.Address = dto.Address;
        userInfo.Contry = dto.Contry;
        userInfo.Email = dto.Email;
        userInfo.PhoneNumber = dto.PhoneNumber;
        userInfo.Role = dto.Role; 
        
        context.UserInfo.Update(userInfo);
        await context.SaveChangesAsync();
        return userInfo;
    }
  
}