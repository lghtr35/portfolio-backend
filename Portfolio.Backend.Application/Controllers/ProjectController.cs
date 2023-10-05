using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Backend.Common.Data.Entities;
using Portfolio.Backend.Common.Data.Requests.Project;
using Portfolio.Backend.Common.Data.Responses.Common;
using Portfolio.Backend.Common.Data.Responses.Project;
using Portfolio.Backend.Services.Interfaces;

namespace Portfolio.Backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;
        public ProjectController(IProjectService projectService, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [RequestSizeLimit(1105199104)]
        public async Task<ActionResult<ProjectResponse>> Create([FromForm] ProjectCreateRequest projectDTO)
        {
            try
            {
                _logger.LogInformation("Create Project recieved a request");
                return Ok(await _projectService.CreateProject(projectDTO));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ProjectController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [HttpGet("latest")]
        public async Task<ActionResult<ProjectResponse?>> ReadLatestProjects([FromQuery] int? count)
        {
            try
            {
                _logger.LogInformation("Read Project recieved a request");
                return Ok(await _projectService.GetProjectsOrderedWithCount(count.GetValueOrDefault(3)));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ProjectController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponse?>> ReadOne(int id)
        {
            try
            {
                _logger.LogInformation("Read Project recieved a request");
                return Ok(await _projectService.GetProject(id));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ProjectController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [HttpGet]
        public async Task<ActionResult<PageResponse<ProjectResponse>>> ReadAll([FromQuery] ProjectFilterRequest dto)
        {
            try
            {
                _logger.LogInformation("Read All Projects recieved a request");
                return Ok(await _projectService.GetProjects(dto));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ProjectController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [Authorize]
        [HttpPut]
        [RequestSizeLimit(1105199104)]
        public async Task<ActionResult<ProjectResponse>> Update([FromBody] ProjectUpdateRequest project)
        {
            try
            {
                _logger.LogInformation("Update Project recieved a request");
                return Ok(await _projectService.UpdateProject(project));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ProjectController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<ProjectResponse>> Delete([FromQuery] int id)
        {
            try
            {
                _logger.LogInformation("Delete Project recieved a request");
                return Ok(await _projectService.DeleteProject(id));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ProjectController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }
    }
}

