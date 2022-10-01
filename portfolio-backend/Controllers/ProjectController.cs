using Microsoft.AspNetCore.Mvc;
using portfolio_backend.Data.Entities;
using portfolio_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace portfolio_backend.Controllers
{
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Project>> Create([FromBody] Project img)
        {
            try
            {
                return Ok(await this._projectService.CreateProject(img));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Project>>> ReadOne(int id)
        {
            try
            {
                return Ok(await this._projectService.GetProject(id));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> ReadAll()
        {
            try
            {
                Dictionary<string, string> query = new();
                string[]? keys = Request.Query.Keys.ToArray();

                if (keys != null)
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        query[keys[i].ToLower()] = Request.Query[keys[i]];
                    }
                }

                return Ok(await this._projectService.GetProjects(query));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<Project>> Update([FromBody] Project img)
        {
            try
            {
                return Ok(await this._projectService.UpdateProject(img));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<Project>> Delete([FromQuery] int[] id)
        {
            try
            {
                return Ok(await this._projectService.DeleteProject(id));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }
    }
}

