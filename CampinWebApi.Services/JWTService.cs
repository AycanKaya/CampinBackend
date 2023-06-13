using CampinWebApi.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CampinWebApi.Core.Models;

namespace CampinWebApi.Services;

public class JWTService : IJWTService
{
    private readonly JWTModel _jwtSettings;
    public JWTService(IOptions<JWTModel> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    public JwtSecurityToken GetToken(IList<Claim> userClaim, IList<string> roles, IdentityUser user)
    {
        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }
        .Union(userClaim)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }

    public IEnumerable<Claim> GetTokenClaims(string tokenStr)
    {
        var token = ValidateToken(tokenStr);

        return token.Claims;
    }

    public JwtSecurityToken ValidateToken(string token)
    {

        if (string.IsNullOrEmpty(token))
            throw new ArgumentNullException(nameof(token));
        var jwt = token.Replace("Bearer ", string.Empty);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

        tokenHandler.ValidateToken(jwt, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero

        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        return jwtToken;

    }

    public string GetUserIdFromJWT(string token)
    {
        var jwt = GetTokenClaims(token);
        var userId = jwt.First(x => x.Type == "uid").Value;
        return userId;
    }
    public string GetUserName(string token)
    {
        var jwt = GetTokenClaims(token);
        var userName = jwt.First(x => x.Type == "sub").Value;
        return userName;
    }
    public string GetUserRole(string token)
    {
        var jwt = GetTokenClaims(token);
        var userRole = jwt.First(x => x.Type == "roles").Value;
        return userRole;
    }


}

