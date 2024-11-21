using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Lemon.Token.Jwt;

public static class JwtUtility
{
    public static class LemonClaimNames
    {
        public const string UserId = "uid";
    }

    public static string GenerateJwtToken(Guid userId, string nickName, DateTime expireTime)
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
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(LemonClaimNames.UserId, userId.ToString()),
            ]
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static bool TryParseToken(string token, out Guid id)
    {
        var handler = new JwtSecurityTokenHandler();
        if (handler.ReadToken(token) is not JwtSecurityToken jsonToken)
        {
            id = Guid.Empty;
            return false;
        }

        id = Guid.Parse(jsonToken.Claims.First(claim => claim.Type == LemonClaimNames.UserId).Value);
        return true;
    }
}