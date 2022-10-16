using portfolio_backend.Services.Interfaces;
using portfolio_backend.Data.Repository;
using portfolio_backend.Data.Entities;
using portfolio_backend.Data.DTOs.Project;
using Microsoft.EntityFrameworkCore;
using portfolio_backend.Data.DTOs.Common;
using System.Linq;
using System.Data.SqlClient;
using portfolio_backend.Data.DTOs.Image;

namespace portfolio_backend.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDatabaseContext _context;
        private readonly IFileUploadService _fileUploadService;
        private readonly IImageService _imageService;
        public ProjectService(AppDatabaseContext context, IFileUploadService fileUploadService,IImageService imageService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _imageService = imageService;
        }

        public async Task<Project> CreateProject(ProjectCreateDTO projectDTO)
        {
            Project project = new Project();
            project.Header = projectDTO.Header;
            project.Message = projectDTO.Message;
            project.Link = projectDTO.Link;
            project.PayloadPath = projectDTO.PayloadPath;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }
        public async Task<PageDTO<Project>> GetProjects(ProjectFilterDTO dto)
        {
            PageDTO<Project> response = new();
            IQueryable<Project> queryable = this._context.Projects;
            queryable = MakeQuery(queryable, dto);
            IEnumerable<Project> list = await queryable.Include(prop=>prop.Images).ToListAsync();
            response.TotalRecords = list.Count();
            int remaining = response.TotalRecords - (dto.Page * dto.Size);
            if (remaining > 0)
            {
                if (dto.Size > remaining)
                {
                    dto.Size = response.TotalRecords - (dto.Page * dto.Size);
                }
                response.PageSize = dto.Size;
                response.PageNumber = dto.Page;
                response.Content = list.Skip(dto.Page * dto.Size).Take(dto.Size);
            }
            return response;
        }
        private static IQueryable<Project> MakeQuery(IQueryable<Project> queryable, ProjectFilterDTO query)
        {
            if (query.IdList != null)
            {
                queryable = queryable.Where(item => query.IdList.Contains(item.ProjectId));
            }
            if (query.PathList != null)
            {
                queryable = queryable.Where(item => query.PathList.Contains(item.PayloadPath));
            }
            if (query.HeaderSearchString != null)
            {
                queryable = queryable.Where(item => item.Header.Contains(query.HeaderSearchString));
            }
            if (query.MessageSearchString != null)
            {
                queryable = queryable.Where(item => item.Message.Contains(query.MessageSearchString));
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

        public async Task<Project?> GetProject(int id)
        {
                Project? res = await _context.Projects.Where(prop => prop.ProjectId == id).Include(prop=>prop.Images).FirstOrDefaultAsync();
                return res;
        }
        public async Task<Project?> UpdateProject(ProjectUpdateDTO projectDTO)
        {

            Project? res = await _context.Projects.Where(prop => prop.ProjectId == projectDTO.ProjectId).Include(prop => prop.Images).FirstOrDefaultAsync();
            if (res == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(projectDTO.Header))
            {
                res.Header = projectDTO.Header;
            }

            if (!string.IsNullOrEmpty(projectDTO.Message))
            {
                res.Message = projectDTO.Message;
            }

            if (!string.IsNullOrEmpty(projectDTO.Link))
            {
                res.Link = projectDTO.Link;
            }

            if(projectDTO.ImageIds != null)
            {
                List<Image> images = new();
                foreach(int imageId in projectDTO.ImageIds)
                {
                    Image? image = await _imageService.GetImage(imageId);
                    if(image != null) {
                        image.Project = res;
                        images.Add(image);
                    }
                    
                }
                res.Images = images.ToList();
            }

            res.UpdatedAt = DateTime.UtcNow;
            _context.Update(res);
            await _context.SaveChangesAsync();
            return res;
        }
        public async Task<Project?> DeleteProject(int id)
        {
            var deleted = await _context.Projects.Where(item => item.ProjectId == id).FirstOrDefaultAsync();
            if (deleted == null)
            {
                return null;
            }
            _context.Remove(deleted);
            await _context.SaveChangesAsync();
            return deleted;
        }
        public async Task<Project?> UploadPayloadToAProject(ProjectUploadDTO dto)
        {
            Project? project = await _context.Projects.FindAsync(dto.ProjectId);
            if (project == null)
            {
                return null;
            }
            string[] accepted = new string[2] { "zip", "rar" };
            List<IFormFile> formFiles = new List<IFormFile>();
            formFiles.Add(dto.ProjectFile);
            string[] paths = await _fileUploadService.UploadWithForm(formFiles, "Projects/", accepted);
            project.PayloadPath = paths[0];
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project?> UploadImageToAProject(ProjectUploadImageDTO dto)
        {
            Project? project = await _context.Projects.Where(prop=>prop.ProjectId == dto.ProjectId).Include(prop=>prop.Images).FirstOrDefaultAsync();
            if (project == null)
            {
                return null;
            }
            ImageUploadDTO imageUploadDTO = new();
            imageUploadDTO.ImageFiles = dto.Files.ToList();
            List<Image> images = (await _imageService.UploadImage(imageUploadDTO)).ToList();
            List<Image> projectImages = project.Images.ToList();
            foreach(Image image in images)
            {
                projectImages.Add(image);
            }
            project.Images = projectImages.ToList();
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return project;
        }
    }
}

