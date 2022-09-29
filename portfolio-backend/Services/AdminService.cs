using System.Threading.Tasks;
using backend.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;


namespace backend.Services
{

    public class AdminService : IAdminService
    {
        private readonly AppDatabaseContext context;
        public AdminService(AppDatabaseContext _context){
            this.context = _context;
        }

        public async Task<Admin> Create(Admin admin){
            this.context.Admins.Add(admin);
            await context.SaveChangesAsync();
            return admin;
        }
        public async Task<Admin> Update() {
            return new Admin();
        }
        public async Task<Admin> Delete() {
            return new Admin();        
        }
        public async Task<Admin> Get(int id) {
            return new Admin();        
        }
        public async Task<Admin> GetAll() {
            return new Admin();       
        }
    }

}