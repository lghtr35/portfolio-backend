using portfolio_backend.Services.Interfaces;

namespace portfolio_backend.Services
{
    public class FileUploadService : IFileUploadService
    {
        public FileUploadService() { }

        public Task<string> Upload(IFormFile file, string path, string[] acceptedExtensions)
        {
            return Task.Run(() =>
            {
                string[] filenameParts = file.FileName.Split(".");
                if (!acceptedExtensions.Contains(filenameParts.Last())) return "";

                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), path);
                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }
                pathBuilt += filenameParts.First();
                using FileStream stream = new(pathBuilt, FileMode.Create);
                file.CopyTo(stream);

                return pathBuilt;
            });
        }
    }
}

