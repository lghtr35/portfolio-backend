using System;
using portfolio_backend.Data.Entities;
using portfolio_backend.Data.DTOs.Project;
using portfolio_backend.Data.DTOs.Common;
using portfolio_backend.Data.DTOs.Image;

namespace portfolio_backend.Services.Interfaces
{
    public interface IProjectService
    {
        Task<Project> CreateProject(ProjectCreateDTO projectDTO);
        Task<PageDTO<Project>> GetProjects(ProjectFilterDTO projectDTO);
        Task<Project?> UpdateProject(ProjectUpdateDTO projectDTO);
        Task<Project?> DeleteProject(int id);
        Task<Project?> GetProject(int id);
        Task<Project?> UploadPayloadToAProject(ProjectUploadDTO dto);
        Task<Project?> UploadImageToAProject(ProjectUploadImageDTO dto);
    }
}

