using System.Data;
using portfolio_backend.Data.Repository;
using portfolio_backend.Data.Entities;
using portfolio_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using portfolio_backend.Data.DTOs.Common;
using portfolio_backend.Data.DTOs.Image;
using System.Collections.Generic;
using portfolio_backend.Data.DTOs.Project;

namespace portfolio_backend.Services
{
    public class ImageService : IImageService
    {
        private readonly AppDatabaseContext _context;
        private readonly IFileUploadService _fileUploadService;
        public ImageService(AppDatabaseContext context, IFileUploadService fileUploadService)
        {
            this._context = context;
            this._fileUploadService = fileUploadService;
        }

        public async Task<Image> CreateImage(Image img)
        {
            if (img.ImageName == null)
            {
                img.ImageName = img.ImagePath.Split("/").Last().Split(".").First();
            }
            _context.Images.Add(img);
            await _context.SaveChangesAsync();
            return img;
        }

        public async Task<PageDTO<Image>> GetImages(ImageFilterDTO query)
        {
            PageDTO<Image> response = new();
            IQueryable<Image> queryable = this._context.Images;
            queryable = MakeQuery(queryable, query);
            IEnumerable<Image> images = await queryable.ToListAsync();
            response.TotalRecords = images.Count();
            int remaining = response.TotalRecords - (query.Page * query.Size);
            if (remaining > 0)
            {
                if (query.Size > remaining)
                {
                    query.Size = response.TotalRecords - (query.Page * query.Size);
                }
                response.PageSize = query.Size;
                response.PageNumber = query.Page;
                response.Content = images.Skip(query.Page * query.Size).Take(query.Size);
            }
            return response;

        }

        private static IQueryable<Image> MakeQuery(IQueryable<Image> queryable, ImageFilterDTO query)
        {
            if (query.IdList != null)
            {
                queryable = queryable.Where(item => query.IdList.Contains(item.ImageId));
            }
            if (query.PathList != null)
            {
                queryable = queryable.Where(item => query.PathList.Contains(item.ImagePath));
            }
            if (query.ImageNameSearchString != null)
            {
                queryable = queryable.Where(item => item.ImageName.Contains(query.ImageNameSearchString));
            }
            if (query.CreatedAtSearchString != null)
            {
                queryable = queryable.Where(item => item.CreatedAt.ToString().Contains(query.CreatedAtSearchString));
            }
            if (query.UpdatedAtSearchString != null)
            {
                queryable = queryable.Where(item => item.UpdatedAt.ToString().Contains(query.UpdatedAtSearchString));
            }
            return queryable;
        }

        public Task<Image?> GetImage(int id)
        {
            return Task.Run(() =>
            {
                Image res = _context.Images.Where(prop => prop.ImageId == id).FirstOrDefault();
                return res;
            });
        }
        public async Task<Image> UpdateImage(ImageUpdateDTO img)
        {
            var res = await _context.Images.FindAsync(img.ImageId);
            if (res == null)
            {
                return null;
            }
            res.UpdatedAt = DateTime.UtcNow;
            Type type = img.GetType();
            foreach (var (prop, newValue) in from prop in type.GetProperties()
                                             let newValue = type.GetProperty(prop.Name).GetValue(img)
                                             where newValue != null
                                             select (prop, newValue))
            {
                type.GetProperty(prop.Name).SetValue(res, newValue);
            }

            await _context.SaveChangesAsync();
            return res;
        }
        public async Task<IEnumerable<Image>> Delete(int[] id)
        {
            var deleted = await _context.Images.Where(item => Array.IndexOf(id, item.ImageId) > -1).ToListAsync();
            _context.Remove(deleted);
            await _context.SaveChangesAsync();
            return deleted;
        }
        public async Task<IEnumerable<Image>> UploadImage(ImageUploadDTO dto)
        {
            List<Image> result = new();
            string[] accepted = new string[5] { "png", "jpg", "jpeg", "gif", "pdf" };
            string[] paths = await _fileUploadService.UploadWithForm(dto.ImageFiles, "../Images", accepted);
            foreach (string path in paths)
            {
                Image image = new Image();
                image.ImagePath = path;
                image = await CreateImage(image);
                result.Add(image);
            }
            return result;
        }
    }
}