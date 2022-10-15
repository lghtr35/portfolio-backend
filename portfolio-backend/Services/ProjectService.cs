using portfolio_backend.Services.Interfaces;
using portfolio_backend.Data.Repository;
using portfolio_backend.Data.Entities;
using portfolio_backend.Data.DTOs.Project;
using Microsoft.EntityFrameworkCore;
using portfolio_backend.Data.DTOs.Common;
using System.Linq;
using System.Data.SqlClient;

namespace portfolio_backend.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDatabaseContext _context;
        private readonly IFileUploadService _fileUploadService;
        public ProjectService(AppDatabaseContext context, IFileUploadService fileUploadService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
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
            IEnumerable<Project> list = await queryable.ToListAsync();
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

        public Task<Project?> GetProject(int id)
        {
            return Task.Run(() =>
            {
                Project? res = _context.Projects.Where(prop => prop.ProjectId == id).FirstOrDefault();
                return res;
            });
        }
        public async Task<Project?> UpdateProject(ProjectUpdateDTO projectDTO)
        {

            var res = await _context.Projects.FindAsync(projectDTO.ProjectId);
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

            if(projectDTO.Images != null)
            {
                res.Images = projectDTO.Images.ToArray();
            }

            res.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return res;
        }
        public async Task<IEnumerable<Project>> DeleteProject(int[] id)
        {
            var deleted = await _context.Projects.Where(item => Array.IndexOf(id, item.ProjectId) > -1).ToListAsync();
            _context.Remove(deleted);
            await _context.SaveChangesAsync();
            return deleted;
        }
        public async Task<Project> UploadPayloadToAProject(ProjectUploadDTO dto)
        {
            Project project = await _context.Projects.Where(pr => pr.ProjectId == dto.ProjectId).FirstOrDefaultAsync();
            if (project == null)
            {
                return null;
            }
            string[] accepted = new string[2] { "zip", "rar" };
            IFormFile[] formFiles = new IFormFile[1] { dto.ProjectFile };
            string[] paths = await _fileUploadService.UploadWithForm(formFiles, "../Projects", accepted);
            project.PayloadPath = paths[0];
            await _context.SaveChangesAsync();
            return project;
        }
    }
}

