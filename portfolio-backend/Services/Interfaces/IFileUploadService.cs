using System;
namespace portfolio_backend.Services.Interfaces
{
    public interface IFileUploadService
    {
        public Task<string[]> UploadWithForm(IFormFile[] files,string path, string[] acceptedExtensions);
    }
}

