using portfolio_backend.Services.Interfaces;
using portfolio_backend.Data.Repository;
using portfolio_backend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace portfolio_backend.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDatabaseContext _context;
        public ProjectService(AppDatabaseContext _context)
        {
            _context = _context;
        }

        public async Task<Project> CreateProject(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }
        public async Task<IEnumerable<Project>> GetProjects(Dictionary<string, string> query)
        {
            if (query.Count == 0)
            {
                IEnumerable<Project> res = await _context.Projects.ToListAsync();
                return res;
            }
            else
            {
                IQueryable<Project> queryable = this._context.Projects;
                queryable = MakeQuery(queryable, query);
                IEnumerable<Project> res = await queryable.ToListAsync();
                return res;
            }
        }
        private static IQueryable<Project> MakeQuery(IQueryable<Project> queryable, Dictionary<string, string> query)
        {
            if (query.ContainsKey("project_id"))
            {
                queryable = queryable.Where(item => item.ProjectId == Int32.Parse(query["project_id"]));
            }
            if (query.ContainsKey("payload_path"))
            {
                queryable = queryable.Where(item => item.PayloadPath == query["payload_path"]);
            }
            if (query.ContainsKey("header"))
            {
                queryable = queryable.Where(item => item.Header == query["header"]);
            }
            if (query.ContainsKey("message"))
            {
                queryable = queryable.Where(item => item.Header == query["message"]);
            }
            if (query.ContainsKey("header"))
            {
                queryable = queryable.Where(item => item.Header == query["header"]);
            }
            if (query.ContainsKey("createdat"))
            {
                queryable = queryable.Where(item => item.CreatedAt.ToString() == query["createdAt"]);
            }
            if (query.ContainsKey("updatedat"))
            {
                queryable = queryable.Where(item => item.UpdatedAt.ToString() == query["updatedat"]);
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
        public async Task<Project?> UpdateProject(Project project)
        {
            project.UpdatedAt = DateTime.UtcNow;
            var res = await _context.Projects.FindAsync(project.ProjectId);
            if (res == null)
            {
                return null;
            }
            Type type = project.GetType();
            foreach (var (prop, newValue) in from prop in type.GetProperties()
                                             let newValue = type.GetProperty(prop.Name).GetValue(project)
                                             where newValue != null
                                             select (prop, newValue))
            {
                type.GetProperty(prop.Name).SetValue(res, newValue);
            }

            await _context.SaveChangesAsync();
            return res;
        }
        public async Task<IEnumerable<Project>> Delete(int[] id)
        {
            var deleted = await _context.Projects.Where(item => Array.IndexOf(id, item.ProjectId) > -1).ToListAsync();
            _context.Remove(deleted);
            await _context.SaveChangesAsync();
            return deleted;
        }

    }
}

