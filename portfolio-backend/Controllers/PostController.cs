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
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpPost]
        public async Task<ActionResult<Post>> Create([FromBody] Post post)
        {
            try
            {
                return Ok(await _postService.Create(post));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }
#nullable enable
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetAll()
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

                return Ok(await _postService.ReadAll(query));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }
#nullable disable
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetOne(int id)
        {
            try
            {
                return Ok(await _postService.ReadOne(id));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Post>> Update([FromBody] Post post)
        {
            try
            {
                return Ok(await _postService.Update(post));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> Delete(int id)
        {
            try
            {
                return Ok(await _postService.Delete(id));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }
    }
}