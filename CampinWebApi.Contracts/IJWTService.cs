using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace CampinWebApi.Contracts
{
    public interface IJWTService
    {
        JwtSecurityToken GetToken(IList<Claim> userClaim, IList<string> roles, IdentityUser users);
        JwtSecurityToken? ValidateToken(string token);
        IEnumerable<Claim> GetTokenClaims(string tokenStr);
        string GetUserIdFromJWT(string token);
        string GetUserName(string token);
        string GetUserRole(string token);
    }
}
