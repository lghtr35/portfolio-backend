using portfolio_backend.Data.DTOs.Common;
using portfolio_backend.Data.DTOs.Image;
using portfolio_backend.Data.Entities;

namespace portfolio_backend.Services.Interfaces
{
    public interface IImageService
    {
        Task<Image> CreateImage(Image img);
        Task<PageDTO<Image>> GetImages(ImageFilterDTO query);
        Task<Image> UpdateImage(ImageUpdateDTO img);
        Task<IEnumerable<Image>> Delete(int[] id);
        Task<Image?> GetImage(int id);
        Task<IEnumerable<Image>> UploadImage(ImageUploadDTO dto);
    }
}