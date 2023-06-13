using System;
using System.Net;
using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO;
using CampinWebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        [HttpPost("authenticate")]
        public async Task<BaseResponseModel<AuthenticationResponseDTO>> Login(AuthenticationRequestDTO request)
        {
            var user = await accountService.Login(request, GenerateIPAddress());
            var result = new BaseResponseModel<AuthenticationResponseDTO>(user, "Logged in");
            return result;

        }

        [HttpPost]
        [Route("register")]
        public async Task<BaseResponseModel<bool>> Register(RegisterRequestDTO request)
        {
            var isSuccedd = await accountService.Register(request);
            var response = new BaseResponseModel<bool>();
            if (isSuccedd)
            {
                response.Succeeded = true;
                response.Message = "User registered . ";

            }
            return response;
        }
        
        [HttpPost]
        [Route("reset-password")]
        public async Task<BaseResponseModel<string>> ResetPassword(ResetPasswordDTO request)
        {
            var response = new BaseResponseModel<string>();
            var isSucced = await accountService.ResetPassword(request);
            if (isSucced)
            {
                response.Succeeded = isSucced;
                response.Message = "Password Change Successful. ";
                response.Errors = "No error";
            }
            return response;
        }

        // Add role to user
        [HttpPost]
        [Route("add-role")]
        public async Task<BaseResponseModel<string>> AddRoleToUser(string roleName)
        {
            var userToken = HttpContext.Request.Headers.Authorization.ToString();
            var response = new BaseResponseModel<string>();
            var isSucced = await accountService.AddRoleToUser(userToken, roleName);
            if (isSucced)
            {
                response.Succeeded = isSucced;
                response.Message = "Role added to user. ";
                response.Errors = "No error";
            }
            return response;
        }
        private string GetToken()
        {
            return HttpContext.Request.Headers.Authorization.ToString();
        }
        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress == null ? "127.0.0.1" : HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }

}