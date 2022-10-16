using portfolio_backend.Services.Interfaces;

namespace portfolio_backend.Services
{
    public class FileUploadService : IFileUploadService
    {
        public FileUploadService() { }

        public Task<string[]> UploadWithForm(IEnumerable<IFormFile> files, string path, string[] acceptedExtensions)
        {
            return Task.Run(() =>
            {
                List<string> result = new();
                foreach(var file in files)
                {
                    string[] filenameParts = file.FileName.Split(".");
                    if (!acceptedExtensions.Contains(filenameParts.Last())) continue;

                    var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), path);
                    if (!Directory.Exists(pathBuilt))
                    {
                        Directory.CreateDirectory(pathBuilt);
                    }
                    pathBuilt += file.FileName;
                    using FileStream stream = new(pathBuilt, FileMode.Create);
                    file.CopyTo(stream);
                    result.Add(pathBuilt);
                }

                return result.ToArray();
            });
        }
    }
}

