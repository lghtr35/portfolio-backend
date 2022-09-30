using portfolio_backend.Models.Entities;

namespace portfolio_backend.Services.Interfaces
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