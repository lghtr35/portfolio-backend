using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using portfolio_backend.Data.Entities;
using portfolio_backend.Services.Interfaces;
using portfolio_backend.Data.DTOs.Project;
using portfolio_backend.Data.DTOs.Common;

namespace portfolio_backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Project>> Create([FromBody] ProjectCreateDTO projectDTO)
        {
            try
            {
                return Ok(await this._projectService.CreateProject(projectDTO));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project?>> ReadOne(int id)
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
        public async Task<ActionResult<PageDTO<Project>>> ReadAll([FromQuery] ProjectFilterDTO dto)
        {
            try
            {
                return Ok(await this._projectService.GetProjects(dto));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<Project>> Update([FromBody] ProjectUpdateDTO project)
        {
            try
            {
                return Ok(await this._projectService.UpdateProject(project));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<Project>> Delete([FromQuery] int id)
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

        [Authorize]
        [HttpPost("upload")]
        public async Task<ActionResult<Project>> UploadProject([FromForm] ProjectUploadDTO dto)
        {
            try
            {
                return Ok(await this._projectService.UploadPayloadToAProject(dto));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,err);
            }
        }
        [Authorize]
        [HttpPost("upload-image")]
        public async Task<ActionResult<Project>> UploadImageToProject([FromForm] ProjectUploadImageDTO dto)
        {
            try
            {
                return Ok(await this._projectService.UploadImageToAProject(dto));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }
    }
}

