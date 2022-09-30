using System.Text;
using portfolio_backend.Services.Interfaces;
using portfolio_backend.Data.Repository;
using System.Security.Cryptography;
using portfolio_backend.Data.Entities;

namespace portfolio_backend.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDatabaseContext context;
        private readonly SHA512 hasher;
        public AdminService(AppDatabaseContext _context)
        {
            hasher = SHA512.Create();
            context = _context;
        }
        private string encryptString(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            bytes = hasher.ComputeHash(bytes);

            return Convert.ToBase64String(bytes);
        }

        public Task<Admin?> GetAdminWithUsernameAndPassword(string username, string password)
        {
            return new Task<Admin?>(() =>
            {
                string hashed = encryptString(password);
                Admin? admin = context.Admins.Where(x => x.Username == username && x.Password == hashed).FirstOrDefault();
                return admin;
            });
        }

        public Task<bool> CreateAdmin(Admin admin)
        {
            return new Task<bool>(() =>
            {
                int count = context.Admins.Count();
                if (count > 0)
                {
                    return false;
                }
                _ = context.Admins.AddAsync(admin);
                context.SaveChangesAsync();
                return true;
            });
        }
    }
}

