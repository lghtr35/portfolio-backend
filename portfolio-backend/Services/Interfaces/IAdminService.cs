using System.Threading.Tasks;
using backend.Models;

namespace backend.Services
{
    public interface IAdminService
    {
        Task<Admin> Create(Admin admin);
        Task<Admin> Delete();
        Task<Admin> Update();
        Task<Admin> Get(int id);
        Task<Admin> GetAll();
    }
}