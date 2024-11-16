using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Lemon.Token.Generator;

public static class JWTGenerator
{
    public static string GenerateToken(Guid userId, string nickName, DateTime expireTime)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JWTToken.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Constants.JWTToken.Issuer,
            audience: nickName,
            expires: expireTime,
            signingCredentials: creds,
            claims: [
                new Claim(JwtRegisteredClaimNames.Sub, Constants.JWTToken.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, userId.ToString()),
            ]
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}