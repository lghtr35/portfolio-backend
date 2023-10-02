using Portfolio.Backend.Common.Data.Entities;
using Portfolio.Backend.Common.Data.Requests.Image;
using Portfolio.Backend.Common.Data.Responses.Common;

namespace Portfolio.Backend.Services.Interfaces;

public interface IImageService
{
    Task<Image> CreateImage(ImageCreateRequest img, Project? project = null);
    Task<PageResponse<Image>> GetImages(ImageFilterRequest query);
    Task<Image?> UpdateImage(ImageUpdateRequest img);
    void Delete(int id);
    Task<Image?> GetImage(int id);
}