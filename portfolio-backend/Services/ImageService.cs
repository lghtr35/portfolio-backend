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

        public async Task<Image?> GetImage(int id)
        {
            Image? res = await _context.Images.Where(prop => prop.ImageId == id).Include(prop=>prop.Project).FirstOrDefaultAsync();
            return res;
        }
        public async Task<Image?> UpdateImage(ImageUpdateDTO img)
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
            if (img.ImagePath != null)
            {
                res.ImagePath = img.ImagePath;
            }
            if (img.ProjectId != null)
            {
                Project? project = await _context.Projects.Where(p => p.ProjectId == img.ProjectId).FirstOrDefaultAsync();
                if (project != null)
                {
                    res.Project = project;
                    List<Image> images = new(project.Images);
                    images.Add(res);
                    project.Images = images.ToList();
                }
            }
            res.UpdatedAt = DateTime.UtcNow;
            _context.Update(res);
            await _context.SaveChangesAsync();
            return res;
        }
        public async Task<Image?> Delete(int id)
        {
            var deleted = await _context.Images.Where(item => item.ImageId == id).FirstOrDefaultAsync();
            if (deleted == null)
            {
                return null;
            }
            _context.Remove(deleted);
            await _context.SaveChangesAsync();
            return deleted;
        }
        public async Task<IEnumerable<Image>> UploadImage(ImageUploadDTO dto)
        {
            List<Image> result = new();
            string[] accepted = new string[5] { "png", "jpg", "jpeg", "gif", "pdf" };
            string[] paths = await _fileUploadService.UploadWithForm(dto.ImageFiles, "Images/", accepted);
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