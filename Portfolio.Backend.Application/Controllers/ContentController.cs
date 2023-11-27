using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Backend.Common;
using Portfolio.Backend.Common.Data.Requests.Content;
using Portfolio.Backend.Common.Data.Responses.Common;
using Portfolio.Backend.Common.Data.Responses.Content;
using Portfolio.Backend.Services.Interfaces;

namespace Portfolio.Backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly ILogger _logger;
        public ContentController(IContentService contentService, ILogger<ContentController> logger)
        {
            _contentService = contentService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ContentResponse>> Create([FromBody] ContentCreateRequest dto)
        {
            try
            {
                _logger.LogInformation("Create Content recieved a request");
                return Ok(await _contentService.CreateContent(dto));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ContentController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [HttpGet("Page/{page}")]
        public async Task<ActionResult<ContentLayoutResponse>> ReadPageContents(string page)
        {
            try
            {
                _logger.LogInformation("Read All Contents recieved a request");
                return Ok(await _contentService.GetPageContent(new ContentGetPageRequest(page)));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ContentController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContentResponse?>> ReadOne(int id)
        {
            try
            {
                _logger.LogInformation("Read Content recieved a request");
                return Ok(await _contentService.GetContent(id));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ContentController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [Authorize]
        [HttpGet("Place")]
        public async Task<ActionResult<IDictionary<string, ContentLayoutResponse>>> ReadAllByPlace()
        {
            try
            {
                _logger.LogInformation("Read All Contents By Place recieved a request");
                return Ok(await _contentService.GetContentsByPlace());
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ContentController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PageResponse<ContentResponse>>> ReadAll([FromQuery] ContentFilterRequest dto)
        {
            try
            {
                _logger.LogInformation("Read All Contents recieved a request");
                return Ok(await _contentService.GetContents(dto));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ContentController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<ContentResponse>> Update([FromBody] ContentUpdateRequest dto)
        {
            try
            {
                _logger.LogInformation("Update Content recieved a request");
                return Ok(await _contentService.UpdateContent(dto));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ContentController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<ContentResponse>> Delete([FromQuery] int id)
        {
            try
            {
                _logger.LogInformation("Delete Content recieved a request");
                return Ok(await _contentService.DeleteContent(id));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ContentController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }
    }
}

