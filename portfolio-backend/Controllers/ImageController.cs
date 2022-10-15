using Microsoft.AspNetCore.Mvc;
using portfolio_backend.Data.Entities;
using portfolio_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using portfolio_backend.Data.DTOs.Common;
using portfolio_backend.Data.DTOs.Image;
using portfolio_backend.Data.DTOs.Project;

namespace portfolio_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<ActionResult<Image>> Create([FromBody] Image img)
        {
            try
            {
                return Ok(await this._imageService.CreateImage(img));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> ReadOne(int id)
        {
            try
            {
                return Ok(await this._imageService.GetImage(id));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }
#nullable enable
        [HttpGet]
        public async Task<ActionResult<PageDTO<Image>>> ReadAll([FromQuery] ImageFilterDTO query)
        {
            try
            {
                return Ok(await this._imageService.GetImages(query));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }
#nullable disable
        [HttpPut]
        public async Task<ActionResult<Image>> Update([FromBody] ImageUpdateDTO img)
        {
            try
            {
                return Ok(await this._imageService.UpdateImage(img));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<Image>> Delete([FromQuery] int[] id)
        {
            try
            {
                return Ok(await this._imageService.Delete(id));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<ActionResult<Project>> UploadImage([FromForm] ImageUploadDTO dto)
        {
            try
            {
                return Ok(await this._imageService.UploadImage(dto));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }
    }
}