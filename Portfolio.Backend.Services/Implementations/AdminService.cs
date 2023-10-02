using Portfolio.Backend.Services.Interfaces;
using Portfolio.Backend.Common.Data.Entities;
using Portfolio.Backend.Common.Data.Repository;
using Portfolio.Backend.Common.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace Portfolio.Backend.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly AppDatabaseContext _context;
        private readonly DatabaseHelper _databaseHelper;
        private readonly SHA512 _hasher;
        public AdminService(AppDatabaseContext context, DatabaseHelper databaseHelper)
        {
            _context = context;
            _databaseHelper = databaseHelper;
            _hasher = SHA512.Create();
        }

        public Task<Admin?> GetAdminWithUsernameAndPassword(string username, string password)
        {
            password = Encoding.UTF8.GetString(_hasher.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return Task.Run(() =>
            {
                Admin? admin = _context.Admins.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
                return admin;
            });
        }

        public Task<bool> CreateAdmin(Admin admin)
        {
            return Task.Run(async () =>
            {
                int count = _context.Admins.Count();
                int threshold = _databaseHelper.RunQueryScalar<int>(@"SELECT dbo.GetAdminThreshold()");
                if (count >= threshold)
                {
                    return false;
                }
                admin.Password = Encoding.UTF8.GetString(_hasher.ComputeHash(Encoding.UTF8.GetBytes(admin.Password)));
                _ = await _context.Admins.AddAsync(admin);
                await _context.SaveChangesAsync();
                return true;
            });
        }
    }
}

