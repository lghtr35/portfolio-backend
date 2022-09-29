using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
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
        public async Task<ActionResult<IEnumerable<Image>>> ReadOne(int id)
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
        public async Task<ActionResult<IEnumerable<Image>>> ReadAll()
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

                return Ok(await this._imageService.GetImages(query));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }
#nullable disable
        [HttpPut]
        public async Task<ActionResult<Image>> Update([FromBody] Image img)
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
    }
}