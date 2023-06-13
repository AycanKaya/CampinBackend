using CampinWebApi.Core.DTO.UserDTO;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Contracts;

public interface IUserInfoService
{
    Task<UserInfo> GetUserInfo(string userToken);
    Task<UserInfo> EditUserInfo(EditUserInfoDTO dto, string userToken);
}