using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Backend.Services.Interfaces;
using Portfolio.Backend.Common.Data.Entities;

namespace Portfolio.Backend.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IAdminService _adminService;

        public AuthenticationService(IConfiguration configuration, IAdminService adminService)
        {
            _adminService = adminService;
            _configuration = configuration;
        }



        public async Task<string?> GenerateToken(string username, string password)
        {
            Admin? admin = await _adminService.GetAdminWithUsernameAndPassword(username, password);
            if (admin == null)
            {
                return null;
            }

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim("UserID", admin.UserId.ToString()),
                        new Claim("Username", admin.Username),
                    };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            var Tvp = new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidIssuer = _configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt").GetValue<string>("Key"))),
            };
            SecurityToken outToken;
            _ = new JwtSecurityTokenHandler().ValidateToken(token, Tvp, out outToken);
            return true;
        }
    }

}