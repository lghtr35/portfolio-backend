using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Backend.Common.Data.Entities;
using Portfolio.Backend.Common.Data.Requests.Auth;
using Portfolio.Backend.Common.Data.Responses.Auth;
using Portfolio.Backend.Services.Interfaces;

namespace Portfolio.Backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger _logger;
        public AdminController(IAdminService adminService, IAuthenticationService authenticationService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        /*
         * Create an admin
         * Create a new admin if the conditions are met
         * 
         */
        [HttpPost]
        public async Task<IActionResult> CreateAdmin([FromBody] LoginRequest adminDTO)
        {
            try
            {
                _logger.LogInformation("Create Admin recieved a request");
                Admin admin = new Admin(adminDTO.Username, adminDTO.Password);
                bool isCreated = await _adminService.CreateAdmin(admin);
                if (isCreated)
                    return Ok(isCreated);
                else
                    return Problem("Admin count is at the threshold", statusCode: StatusCodes.Status405MethodNotAllowed);
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in AdminController due to: " + err.Message, err);
                return Problem(detail: "An error occured in AdminController due to: ", err.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /*
         * Login as an admin
         * Give credentials to obtain a jwt token
         * 
         */
        [HttpPost("/login")]
        public async Task<ActionResult<LoginResponse>> Authenticate([FromBody] LoginRequest admin)
        {
            try
            {
                _logger.LogInformation("Authenticate Admin recieved a request");
                var token = await _authenticationService.GenerateToken(admin.Username, admin.Password);
                if (string.IsNullOrEmpty(token)) throw new UnauthorizedAccessException("unsucsessful login with given info, check request");
                var adminLogin = new LoginResponse();
                adminLogin.Token = token;
                adminLogin.Username = admin.Username;
                return Ok(adminLogin);
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in AdminController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }
    }
}

