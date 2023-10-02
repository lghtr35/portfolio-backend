namespace Portfolio.Backend.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string?> GenerateToken(string username, string password);
    }
}

