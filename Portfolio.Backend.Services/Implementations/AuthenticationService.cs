using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly IAdminService adminService;

        public AuthenticationService(IConfiguration _configuration, IAdminService _adminService)
        {
            adminService = _adminService;
            this._configuration = _configuration;
        }



        public async Task<string?> GenerateToken(string username, string password)
        {
            Admin? admin = await adminService.GetAdminWithUsernameAndPassword(username, password);
            if (admin == null)
            {
                return null;
            }

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserID", admin.UserId.ToString()),
                        new Claim("Username", admin.Username),
                    };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(45),
                signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}