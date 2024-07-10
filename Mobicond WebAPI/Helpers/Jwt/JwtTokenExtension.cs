using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mobicond_WebAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mobicond_WebAPI.Helpers.Jwt
{
    public static class JwtTokenExtension
    {
        public static string GenerateJWTToken(User user, JwtSettings jwtSettings)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.RoleCode.ToString()),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                signingCredentials: credentials,
                notBefore: DateTime.Now,
                expires: DateTime.Now.Add(TimeSpan.FromHours(6))
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
