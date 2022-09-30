using System;
namespace portfolio_backend.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string?> GenerateToken(string username,string password);
    }
}

