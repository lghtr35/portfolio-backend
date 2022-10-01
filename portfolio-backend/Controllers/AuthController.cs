using Microsoft.AspNetCore.Mvc;
using portfolio_backend.Data.DTOs;
using portfolio_backend.Services.Interfaces;

namespace portfolio_backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService) {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<ActionResult<TokenDTO>> Authenticate([FromBody] AdminDTO admin)
        {
            try
            {
                return Ok(await _authenticationService.GenerateToken(admin.Username, admin.Password));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }


    }
}