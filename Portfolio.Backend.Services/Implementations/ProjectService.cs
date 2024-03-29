﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Backend.Common.Data.Entities;
using Portfolio.Backend.Common.Data.Repository;
using Portfolio.Backend.Common.Data.Requests.Image;
using Portfolio.Backend.Common.Data.Requests.Project;
using Portfolio.Backend.Common.Data.Responses.Common;
using Portfolio.Backend.Common.Data.Responses.Project;
using Portfolio.Backend.Common.Exceptions;
using Portfolio.Backend.Common.Helpers;
using Portfolio.Backend.Services.Interfaces;

namespace Portfolio.Backend.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly AppDatabaseContext _context;
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;
        private readonly string[] _accepted;
        private readonly string _saveDirectory;

        public ProjectService(AppDatabaseContext context, IImageService imageService, IConfiguration configuration)
        {
            _context = context;
            _imageService = imageService;
            _configuration = configuration;
            _accepted = _configuration.GetSection("AcceptedExtensions").GetSection("Project").Get<string[]>();
            _saveDirectory = _configuration.GetSection("DirectoryPaths").GetValue<string>("Project");
        }

        public async Task<ProjectResponse> CreateProject(ProjectCreateRequest projectDTO)
        {
            Console.WriteLine("Img Count: {0}", projectDTO.ProjectImages?.Count() ?? 0);
            Project project = new Project();
            project.Header = projectDTO.Header;
            project.Message = projectDTO.Message;
            project.Link = projectDTO.Link;
            project.IsDownloadable = projectDTO.IsDownloadable;
            var imgs = new List<Image>();
            if (!QueryConditionManager.IsFileExtensionAccepted(_accepted, projectDTO.ProjectFile))
            {
                throw new FileExtensionNotAcceptedException($"{projectDTO.ProjectFile.FileName} has an extension that is not accepted by this service");
            }
            project.PayloadPath = FileHelper.CreateFileAndFolder(_saveDirectory, projectDTO.ProjectFile);
            if (!projectDTO.ProjectImages.IsNullOrEmpty())
            {
                foreach (IFormFile imgFile in projectDTO.ProjectImages)
                {
                    ImageCreateRequest req = new()
                    {
                        ImageName = string.Format("{0}_{1}", projectDTO.Header, imgFile.FileName),
                        ImageFile = imgFile
                    };
                    Image img = await _imageService.CreateImage(req, project);
                    Console.WriteLine("Image {0} => Path {1}", img.ImageName, img.PayloadPath);
                    imgs.Add(img);
                }
            }
            project.Images = imgs.ToArray();
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return new ProjectResponse(project);
        }
        public async Task<PageResponse<ProjectResponse>> GetProjects(ProjectFilterRequest dto)
        {
            PageResponse<ProjectResponse> response = new();
            IQueryable<Project> queryable = _context.Projects;
            queryable = MakeQuery(queryable, dto);
            IEnumerable<ProjectResponse> list = await queryable.Include(prop => prop.Images
            ).Select(p => new ProjectResponse(p)).ToListAsync();
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
        private static IQueryable<Project> MakeQuery(IQueryable<Project> queryable, ProjectFilterRequest query)
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
            return queryable.OrderBy(p => p.CreatedAt);
        }

        public async Task<ProjectResponse?> GetProject(int id)
        {
            Project? res = await _context.Projects.Where(prop => prop.ProjectId == id).Include(prop => prop.Images).FirstOrDefaultAsync();
            if (res == null)
            {
                throw new ObjectNotFoundException("No project with the given id have been found");
            }
            return new ProjectResponse(res);
        }
        public async Task<ProjectResponse?> UpdateProject(ProjectUpdateRequest projectDTO)
        {

            Project? res = await _context.Projects.Where(prop => prop.ProjectId == projectDTO.ProjectId).Include(prop => prop.Images).FirstOrDefaultAsync();
            if (res == null)
            {
                throw new ObjectNotFoundException("No project with the given id have been found");
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

            res.IsDownloadable = projectDTO.IsDownloadable.GetValueOrDefault(res.IsDownloadable);

            if (projectDTO.ProjectFile != null)
            {
                if (!QueryConditionManager.IsFileExtensionAccepted(_accepted, projectDTO.ProjectFile))
                {
                    throw new FileExtensionNotAcceptedException($"{projectDTO.ProjectFile.FileName} has an extension that is not accepted by this service");
                }
                FileHelper.DeleteFile(res.PayloadPath);
                res.PayloadPath = FileHelper.CreateFile(_saveDirectory, projectDTO.ProjectFile);
            }

            res.UpdatedAt = DateTime.UtcNow;
            _context.Update(res);
            await _context.SaveChangesAsync();
            return new ProjectResponse(res);
        }

        public async Task<ProjectResponse?> DeleteProject(int id)
        {
            var deleted = await _context.Projects.Where(item => item.ProjectId == id).FirstOrDefaultAsync();
            if (deleted == null)
            {
                throw new ObjectNotFoundException("No project with the given id have been found");
            }
            foreach (Image image in deleted.Images)
            {
                await _imageService.DeleteImage(image.ImageId);
            }
            FileHelper.DeleteFile(deleted.PayloadPath);
            _context.Remove(deleted);
            await _context.SaveChangesAsync();
            return new ProjectResponse(deleted);
        }

        public async Task<ProjectResponse?> UploadImageToProject(ProjectUploadImageRequest dto)
        {
            Project? project = await _context.Projects.Where(prop => prop.ProjectId == dto.ProjectId).Include(prop => prop.Images).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ObjectNotFoundException("No project with the given id have been found");
            }
            List<Image> images = new();
            foreach (var img in dto.Images)
            {
                ImageCreateRequest createRequest = new();
                createRequest.ImageName = img.Item1;
                createRequest.ImageFile = img.Item2;
                images.Add(await _imageService.CreateImage(createRequest, project));
            }
            List<Image> projectImages = project.Images.ToList();
            foreach (Image image in images)
            {
                projectImages.Add(image);
            }
            project.Images = projectImages.ToArray();
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return new ProjectResponse(project);
        }

        public async Task<ProjectResponse?> DeleteImageFromProject(ProjectDeleteImageRequest dto)
        {
            Project? project = await _context.Projects.Where(prop => prop.ProjectId == dto.ProjectId).Include(prop => prop.Images).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ObjectNotFoundException("No project with the given id have been found");
            }
            List<Image> projectImages = project.Images.ToList();
            projectImages.RemoveAll(s => dto.ImageIds.Contains(s.ImageId));
            project.Images = projectImages.ToArray();
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return new ProjectResponse(project);
        }

        public async Task<PageResponse<ProjectResponse>> GetProjectsLatestWithCount(int count)
        {
            PageResponse<ProjectResponse> response = new();
            IQueryable<Project> queryable = _context.Projects;
            IEnumerable<ProjectResponse> list = await queryable
                .OrderByDescending(p => p.UpdatedAt)
                .Include(prop => prop.Images)
                .Where(p => p.Images.Count > 0)
                .Select(p => new ProjectResponse(p))
                .Take(count)
                .ToListAsync();
            response.TotalRecords = list.Count();
            if (response.TotalRecords <= count)
            {
                response.Content = list.Take(count).ToArray();
                response.ItemsInPage = count;
                response.PageSize = count;
            }
            else
            {
                response.Content = list.Take(response.TotalRecords).ToArray();
                response.ItemsInPage = response.TotalRecords;
                response.PageSize = response.TotalRecords;
            }
            response.PageNumber = 0;
            return response;
        }

        public async Task<ProjectFileResponse> GetProjectFile(int id)
        {
            Project? project = await _context.Projects.Where(p => p.ProjectId == id).FirstOrDefaultAsync();
            if (project == null) throw new ObjectNotFoundException("No project with the given id have been found");
            if (!project.IsDownloadable) throw new UnauthorizedAccessException("This project is not downloadable");

            return new ProjectFileResponse(project);
        }
    }
}

