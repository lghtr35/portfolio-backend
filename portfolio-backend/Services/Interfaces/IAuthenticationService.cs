using System;
namespace portfolio_backend.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<String> generateToken(String username,String password);
    }
}

