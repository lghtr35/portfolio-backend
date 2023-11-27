using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portfolio.Backend.Services.Interfaces;
using Portfolio.Backend.Common.Data.Entities;
using Portfolio.Backend.Common.Exceptions;
using Portfolio.Backend.Common.Helpers;
using Portfolio.Backend.Common.Data.Repository;
using System.Net.Http.Json;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Backend.Common.Data.Responses.Common;
using Portfolio.Backend.Common.Data.Requests.Image;

namespace Portfolio.Backend.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly AppDatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly string[] _accepted;
        private readonly string _saveDirectory;

        public ImageService(AppDatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _accepted = _configuration.GetSection("AcceptedExtensions").GetSection("Image").Get<string[]>();
            _saveDirectory = _configuration.GetSection("DirectoryPaths").GetValue<string>("Image");
        }

        public async Task<Image> CreateImage(ImageCreateRequest img, Project? project = null)
        {
            Image image = new();
            if (!QueryConditionManager.IsFileExtensionAccepted(_accepted, img.ImageFile))
            {
                throw new FileExtensionNotAcceptedException($"{img.ImageFile.FileName} has an extension that is not accepted by this service");
            }

            image.ImageName = img.ImageName;
            if (project != null)
            {
                image.PayloadPath = FileHelper.CreateFileAndFolder(string.Format("{0}/{1}", _saveDirectory, project.Header.Replace(" ", "_")), img.ImageFile);
            }
            else
            {
                image.PayloadPath = FileHelper.CreateFileAndFolder(string.Format("{0}/{1}", _saveDirectory, "Common"), img.ImageFile);
            }
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<PageResponse<Image>> GetImages(ImageFilterRequest dto)
        {
            PageResponse<Image> response = new();
            IQueryable<Image> queryable = _context.Images;
            queryable = MakeQuery(queryable, dto);
            IEnumerable<Image> list = await queryable.ToListAsync();
            response.TotalRecords = list.Count();
            int remaining = response.TotalRecords - dto.Page * dto.Size;
            response.Content = list.Skip(dto.Page * dto.Size).Take(dto.Size).ToArray();
            response.ItemsInPage = dto.Size;
            if (dto.Size > remaining)
            {
                response.ItemsInPage = response.TotalRecords - dto.Page * dto.Size;
            }
            response.PageSize = dto.Size;
            response.PageNumber = dto.Page;
            return response;

        }

        private static IQueryable<Image> MakeQuery(IQueryable<Image> queryable, ImageFilterRequest query)
        {
            if (query.IdList != null)
            {
                queryable = queryable.Where(item => query.IdList.Contains(item.ImageId));
            }
            if (query.PathList != null)
            {
                queryable = queryable.Where(item => query.PathList.Contains(item.PayloadPath));
            }
            if (query.ImageNameSearchString != null)
            {
                queryable = queryable.Where(item => item.ImageName.Contains(query.ImageNameSearchString));
            }
            return queryable;
        }

        public async Task<Image?> GetImage(int id)
        {
            Image? res = await _context.Images.Where(prop => prop.ImageId == id).FirstOrDefaultAsync();
            if (res == null)
            {
                throw new ObjectNotFoundException("No project with the given id have been found");
            }
            return res;
        }

        public async Task<Image> GetImageWithName(string name)
        {
            Image? res = await _context.Images.Where(prop => prop.ImageName == name).FirstOrDefaultAsync();
            if (res == null)
            {
                throw new ObjectNotFoundException("No project with the given id have been found");
            }
            return res;
        }

        public async Task<Image?> UpdateImage(ImageUpdateRequest img)
        {
            var res = await _context.Images.Where(prop => prop.ImageId == img.ImageId).Include(prop => prop.Project).FirstOrDefaultAsync();
            if (res == null)
            {
                return null;
            }
            if (img.ImageName != null)
            {
                res.ImageName = img.ImageName;
            }
            if (img.ImageFile != null)
            {
                if (!QueryConditionManager.IsFileExtensionAccepted(_accepted, img.ImageFile))
                {
                    throw new FileExtensionNotAcceptedException($"{img.ImageFile.FileName} has an extension that is not accepted by this service");
                }
                FileHelper.DeleteFile(res.PayloadPath);
                res.PayloadPath = FileHelper.CreateFile(_saveDirectory, img.ImageFile);
            }
            if (img.ProjectId != null)
            {
                Project? project = await _context.Projects.Where(p => p.ProjectId == img.ProjectId).FirstOrDefaultAsync();
                if (project != null)
                {
                    res.Project = project;
                    List<Image> images = new(project.Images)
                    {
                        res
                    };
                    project.Images = images.ToArray();
                }
            }
            res.UpdatedAt = DateTime.UtcNow;
            _context.Update(res);
            await _context.SaveChangesAsync();
            return res;
        }
        public async Task DeleteImage(int id)
        {
            var deleted = await _context.Images.Where(item => item.ImageId == id).FirstOrDefaultAsync();
            if (deleted == null)
            {
                throw new ObjectNotFoundException("No Image with given id found!");
            }
            FileHelper.DeleteFile(deleted.PayloadPath);
            _context.Remove(deleted);
            await _context.SaveChangesAsync();
        }
    }
}