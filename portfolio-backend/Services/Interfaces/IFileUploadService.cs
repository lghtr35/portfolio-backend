using System;
namespace portfolio_backend.Services.Interfaces
{
    public interface IFileUploadService
    {
        public Task<string[]> UploadWithForm(IEnumerable<IFormFile> files,string path, string[] acceptedExtensions);
    }
}

