using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Cryptography;
using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO;
using CampinWebApi.Core.DTO.UserDTO;
using CampinWebApi.Core.Models;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CampinWebApi.Services
{
    public class AccountService : IAccountService
	{
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IJWTService jwtService;
        private readonly CampinDbContext context;

        public AccountService(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager, IJWTService service, CampinDbContext context)
        {
            this.userManager = userManager;
            jwtService = service;
            this.signInManager = signInManager;
            this.context = context;
        }

        public async Task<bool> Register(RegisterRequestDTO registerRequest)
        {
            var exist_user = await userManager.FindByEmailAsync(registerRequest.Email);
            if (exist_user != null)
                throw new BadHttpRequestException($"Username '{registerRequest.Email}' is already taken.");
            
            if (isValidEmail(registerRequest.Email) && isValidatePassword(registerRequest.Password))
            {
                IdentityUser user = new()
                {
                    Email = registerRequest.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = registerRequest.Name + Guid.NewGuid(),
                };
                var result = await userManager.CreateAsync(user, registerRequest.Password);

                if (!result.Succeeded)
                    throw new Exception($"{result.Errors}");

                await userManager.AddToRoleAsync(user, "Basic");
                
                var c = new CultureInfo("en-GB");
                var r = new RegionInfo(c.LCID);
                string name = r.Name;

                var userInfo = new UserInfo();
                userInfo.Name = registerRequest.Name;
                userInfo.Surname = registerRequest.Surname;
                userInfo.PhoneNumber = registerRequest.PhoneNumber;
                userInfo.Email= registerRequest.Email;
                userInfo.UserID=user.Id;
                userInfo.Role = "Basic";
                userInfo.Address = r.TwoLetterISORegionName;
                userInfo.Contry = r.TwoLetterISORegionName;
                context.UserInfo.Add(userInfo);
                await context.SaveChanges(); 
                return true;
            }
            else
                throw new BadHttpRequestException("Invalid email or password! Your password must contain uppercase and lowercase letters and at least one punctuation mark.");
        }
        
        // Add role to user
        public async Task<bool> AddRoleToUser(string userToken, string role)
        {
            var userId = jwtService.GetUserIdFromJWT(userToken);
            var user = await userManager.FindByIdAsync(userId);
            
            if (user == null)
                throw new FileNotFoundException($"User not be found");
            
            await userManager.AddToRoleAsync(user, role);
            return true;
        }

        public async Task<AuthenticationResponseDTO> Login(AuthenticationRequestDTO authenticationRequest, string ipAddress)
        {
            var user = await userManager.FindByEmailAsync(authenticationRequest.Email);
            if (user == null)
                throw new Exception($"User not be found");

            var result = await signInManager.PasswordSignInAsync(user.UserName, authenticationRequest.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new BadHttpRequestException($"Invalid Credentials for '{authenticationRequest.Email}'.");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            JwtSecurityToken token = jwtService.GetToken(userClaims, roles, user);
            var userInfo= await context.UserInfo.Where(x => x.UserID == user.Id).FirstOrDefaultAsync();

            AuthenticationResponseDTO dto = new AuthenticationResponseDTO();
            dto.Id = user.Id;
            dto.JWToken = new JwtSecurityTokenHandler().WriteToken(token);
            dto.Email = user.Email;
            dto.Name = userInfo.Name;
            dto.Surname = userInfo.Surname;
            var rolesList = await userManager.GetRolesAsync(user).ConfigureAwait(false);
            dto.Roles = rolesList.ToList();
            dto.IsVerified = user.EmailConfirmed;
            var refreshToken = GenerateRefreshToken(ipAddress);
            dto.RefreshToken = refreshToken.Token;
            return dto;
        }
        
        public async Task<UserInfo> SettingUserInfo(UserInfoDTO dto, string token)
        {
            var userId = jwtService.GetUserIdFromJWT(token);
            var existInfo = context.UserInfo.Where(x => x.UserID == userId).FirstOrDefault();
            var user = await userManager.FindByIdAsync(userId);
            var userEmail = await userManager.GetEmailAsync(user);
            await userManager.SetEmailAsync(user, dto.Email);

            if (existInfo == null)
            {
                var userInfo = new UserInfo();
                userInfo = SetUserInfo(dto, userInfo, token, userId);
                context.UserInfo.Add(userInfo);
                await context.SaveChanges();
                return userInfo;
            }
            existInfo = SetUserInfo(dto, existInfo, token, userId);
            await context.SaveChanges();
            return existInfo;
        }
        public async Task<UserInfo> GetUserInfo(string token)
        {
            var userId = jwtService.GetUserIdFromJWT(token);
            var userInfo = context.UserInfo.Where(x => x.UserID == userId).FirstOrDefault();
            return userInfo;
        }
        public async Task<bool> ResetPassword(ResetPasswordDTO resetPassword)
        {
            var account = await userManager.FindByEmailAsync(resetPassword.Email);
            if (account == null) throw new FileNotFoundException("User not be found, please check your email address.");
            
            if (resetPassword.Password != resetPassword.ConfirmPassword)
                throw new BadHttpRequestException("Passwords not match ! ");

            var result = await userManager.ChangePasswordAsync(account, resetPassword.OldPassword, resetPassword.Password);
            if (!result.Succeeded)
                throw new BadHttpRequestException("Old password not match!");
            return true;

        }
        private UserInfo SetUserInfo(UserInfoDTO dto, UserInfo userInfo, string token, string userId)
        {
            userInfo.UserID = userId;
            userInfo.Role = jwtService.GetUserRole(token);
            userInfo.Surname = dto.Surname;
            userInfo.Name = dto.Name;
            userInfo.Email = dto.Email;
            userInfo.Gender = dto.Gender;
            userInfo.Contry = dto.Contry;
            userInfo.Address = dto.Address;
            userInfo.PhoneNumber = dto.PhoneNumber;
            return userInfo;
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
        private bool isValidEmail(string email)
        {

            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var emailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }

        }

        private bool isValidatePassword(string passWord)
        {
            if (string.IsNullOrEmpty(passWord) || passWord.Length < 8)
                return false;
            int validConditions = 0;
            foreach (char c in passWord)
            {
                if (c >= 'a' && c <= 'z')
                {
                    validConditions++;
                    break;
                }
            }
            foreach (char c in passWord)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 1) return false;
            foreach (char c in passWord)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 2) return false;
            return true;
        }

    }
}

