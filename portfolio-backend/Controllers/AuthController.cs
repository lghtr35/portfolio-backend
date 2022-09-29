// using System;
// using System.Threading.Tasks;
// using backend.Models;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;

// namespace backend.Controllers
// {
//     [ApiController]
//     [Route("api/v1/[controller]")]
//     public class AuthController : ControllerBase
//     {
//         public AuthController() { }

//         [HttpGet]
//         public async Task<ActionResult<Admin>> Authenticate([FromBody] Admin admin)
//         {
//             try
//             {
//                 return Ok(await Create());
//             }
//             catch (Exception err)
//             {
//                 return StatusCode(StatusCodes.Status500InternalServerError, err);
//             }
//         }


//     }
// }