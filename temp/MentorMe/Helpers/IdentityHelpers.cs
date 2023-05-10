using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Enums;
using Domain.Identity;
using Microsoft.IdentityModel.Tokens;
using Public.DTO.v1.Identity;

namespace Helpers;

public static class IdentityHelpers
{
    // extension function
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        return Guid.Parse(
            user.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
    
    public static string GenerateJwt(IEnumerable<Claim> claims, string key,
        string issuer, string audience, string mobilePhone, string userType,
        int expiresInSeconds)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddSeconds(expiresInSeconds);
        
        var claimsList = claims.ToList();
        claimsList.Add(new Claim(ClaimTypes.MobilePhone, mobilePhone));
        claimsList.Add(new Claim("UserType", userType));
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claimsList,
            expires: expires,
            signingCredentials: signingCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static bool ValidateToken(string jwt,  string key,
        string issuer, string audience, bool ignoreExpiration = true)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters(key, issuer, audience);

        SecurityToken validatedToken;
        try
        {
            var principal = tokenHandler.ValidateToken(jwt, validationParameters, out validatedToken);
        }
        catch (SecurityTokenExpiredException)
        {
            // is it ok to be expired? since we are refreshing expired jwt
            return ignoreExpiration;
        }
        catch (Exception)
        {
            // something else was wrong
            return false;
        }

        return true;
    }

    private static TokenValidationParameters GetValidationParameters(string key,
        string issuer, string audience)
    {
        return new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidIssuer = issuer,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true
        };
    }
    
    public static AppUser? FilterOutUsers(bool isTutor, List<AppUser> appUsers)
    {
        foreach (var user in appUsers)
        {
            switch (isTutor)
            {
                case true 
                    when user.AppUserType == EUserType.Tutor:
                    return user;
                case false 
                    when user.AppUserType == EUserType.Student:
                    return user;
            }
        }
        return null;
    }
}
