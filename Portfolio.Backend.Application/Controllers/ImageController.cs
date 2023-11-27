using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Backend.Common.Data.Requests.Image;
using Portfolio.Backend.Common.Data.Responses.Common;
using Portfolio.Backend.Common.Data.Responses.Image;
using Portfolio.Backend.Services.Interfaces;

// For more information on enabling Web API for empty Images, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Portfolio.Backend.Controllers
{
    [Route("api/v1/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly ILogger _logger;
        public ImageController(IImageService ImageService, ILogger<ImageController> logger)
        {
            _imageService = ImageService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [RequestSizeLimit(1105199104)]
        public async Task<ActionResult<ImageResponse>> Create([FromForm] ImageCreateRequest dto)
        {
            try
            {
                _logger.LogInformation("Create Image recieved a request");
                return Ok(await _imageService.CreateImage(dto));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ImageController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<ImageResponse>> ReadOne(string name)
        {
            try
            {
                _logger.LogInformation("Read Image recieved a request");
                var image = await _imageService.GetImageWithName(name);
                if (image == null) throw new BadHttpRequestException("No image with given name");
                var res = new ImageResponse(image);
                return Ok(res);
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ImageController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PageResponse<ImageResponse>>> ReadAll([FromQuery] ImageFilterRequest dto)
        {
            try
            {
                _logger.LogInformation("Read All Images recieved a request");
                var res = await _imageService.GetImages(dto);
                if (res.ItemsInPage == 0) throw new BadHttpRequestException("Filter returns no results");
                return Ok(res);
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ImageController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [Authorize]
        [HttpPut]
        [RequestSizeLimit(1105199104)]
        public async Task<ActionResult<ImageResponse>> Update([FromForm] ImageUpdateRequest Image)
        {
            try
            {
                _logger.LogInformation("Update Image recieved a request");
                return Ok(await _imageService.UpdateImage(Image));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ImageController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] int id)
        {
            try
            {
                _logger.LogInformation("Delete Image recieved a request");
                await _imageService.DeleteImage(id);
                return Ok();
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in ImageController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }
    }
}

