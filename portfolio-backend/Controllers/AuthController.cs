using Microsoft.AspNetCore.Mvc;
using portfolio_backend.Data.DTOs;
using portfolio_backend.Services.Interfaces;

namespace portfolio_backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        public AuthController(IAuthenticationService _authenticationService) {
            authenticationService = _authenticationService;
        }

        [HttpGet]
        public async Task<ActionResult<TokenDTO>> Authenticate([FromBody] AdminDTO admin)
        {
            try
            {
                return Ok(await authenticationService.GenerateToken(admin.Username, admin.Password));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }


    }
}