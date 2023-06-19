using System;
using CampinWebApi.Core.DTO;
using CampinWebApi.Core.DTO.UserDTO;
using CampinWebApi.Core.Models.AccountModels;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Contracts
{
    public interface IAccountService
    {
        Task<bool> Register(RegisterRequestDTO registerRequest);
        Task<LoginResponseModel> Login(AuthenticationRequestDTO authenticationRequest, string ipAddress);
        Task<bool> ResetPassword(ResetPasswordDTO resetPassword);
        Task<UserInfo> GetUserInfo(string token);
        Task<UserInfo> SettingUserInfo(UserInfoDTO dto, string token);
        Task<bool> AddRoleToUser(string userToken, string role);
    }
}

