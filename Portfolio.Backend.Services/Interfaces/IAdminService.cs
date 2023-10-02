using Portfolio.Backend.Common.Data.Entities;

namespace Portfolio.Backend.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<bool> CreateAdmin(Admin admin);
        public Task<Admin?> GetAdminWithUsernameAndPassword(string username, string password);
    }
}

