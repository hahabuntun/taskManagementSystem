using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Vkr.Application.Interfaces.Infractructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Vkr.Domain.Entities;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Infractructure
{
    public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public string GenerateToken(Workers user)
        {
            Claim[] claims = [
                new Claim("canManageWorkers", user.CanManageWorkers.ToString()), 
                new Claim("canManageProjects", user.CanManageProjects.ToString()),
                new Claim("userId", user.Id.ToString())
            ];
            
            var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiresHours));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return "Bearer " + tokenValue;
        }
    }
}
