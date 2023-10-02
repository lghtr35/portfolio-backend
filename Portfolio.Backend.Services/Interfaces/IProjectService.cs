using Portfolio.Backend.Common.Data.Requests.Project;
using Portfolio.Backend.Common.Data.Responses.Common;
using Portfolio.Backend.Common.Data.Responses.Project;

namespace Portfolio.Backend.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectResponse> CreateProject(ProjectCreateRequest projectDTO);
        Task<PageResponse<ProjectResponse>> GetProjects(ProjectFilterRequest projectDTO);
        Task<ProjectResponse?> UpdateProject(ProjectUpdateRequest projectDTO);
        Task<ProjectResponse?> DeleteProject(int id);
        Task<ProjectResponse?> GetProject(int id);
        Task<ProjectResponse?> UploadImageToProject(ProjectUploadImageRequest dto);
        Task<ProjectResponse?> DeleteImageFromProject(ProjectDeleteImageRequest dto);
    }
}

