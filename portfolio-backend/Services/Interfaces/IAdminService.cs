using portfolio_backend.Data.Entities;
namespace portfolio_backend.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<bool> CreateAdmin(Admin admin);
        public Task<Admin?> GetAdminWithUsernameAndPassword(string username,string password);
        public void IncrementThreshold();
    }
}

