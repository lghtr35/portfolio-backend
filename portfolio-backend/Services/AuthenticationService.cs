using portfolio_backend.Data.Repository;
using portfolio_backend.Data.Entities;
using portfolio_backend.Services.Interfaces;
using System.Data.Entity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;

namespace portfolio_backend.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration configuration;
        private readonly SHA512 hasher;
        private readonly IAdminService adminService;

        public AuthenticationService(IConfiguration _configuration, IAdminService _adminService)
        {
            this.adminService = _adminService;
            this.hasher = SHA512.Create();
            this.configuration = _configuration;
        }



        public async Task<string?> GenerateToken(string username, string password)
        {
            Admin? admin = await adminService.GetAdminWithUsernameAndPassword(username,password);
            if (admin == null)
            {
                return null;
            }

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserID", admin.UserId.ToString()),
                        new Claim("Username", admin.Username),
                    };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(45),
                signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}