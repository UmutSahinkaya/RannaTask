using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Helpers
{
    public class TokenManager : ITokenService
    {
        private readonly JwtOptions _jwtOptions;

        public TokenManager(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateTokenUser(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var credentials= new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Username),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var token = new JwtSecurityToken(_jwtOptions.Issuer, _jwtOptions.Audience, claims, null, DateTime.UtcNow.AddMinutes(60), credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateTokenCustomer(Customer customer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,customer.Id.ToString()),
                new Claim("IsCustomer","true"),
                new Claim(ClaimTypes.Name,"Customer")
            };
            var token = new JwtSecurityToken(_jwtOptions.Issuer, _jwtOptions.Audience, claims,null, DateTime.UtcNow.AddMinutes(60),credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
