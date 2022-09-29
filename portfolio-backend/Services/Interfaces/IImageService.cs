using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Services
{
    public interface IImageService
    {
        Task<Image> CreateImage(Image img);
        Task<IEnumerable<Image>> GetImages(Dictionary<string, string> query);
        Task<Image> UpdateImage(Image img);
        Task<IEnumerable<Image>> Delete(int[] id);
        Task<Image> GetImage(int id);
    }
}