using System;
using portfolio_backend.Data.Entities;

namespace portfolio_backend.Services.Interfaces
{
    public interface IProjectService
    {
        Task<Project> CreateProject(Project project);
        Task<IEnumerable<Project>> GetProjects(Dictionary<string, string> query);
        Task<Project?> UpdateProject(Project project);
        Task<IEnumerable<Project>> DeleteProject(int[] id);
        Task<Project?> GetProject(int id);
    }
}

