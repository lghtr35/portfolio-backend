using Microsoft.AspNetCore.Mvc;
using portfolio_backend.Data.Entities;
using portfolio_backend.Data.DTOs;
using portfolio_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace portfolio_backend.Controllers
{ 
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(AdminDTO adminDTO)
        {
            try
            {
                Admin admin = new Admin(adminDTO.Username, adminDTO.Password);
                bool isCreated = await _adminService.CreateAdmin(admin);
                if (isCreated)
                    return Ok(isCreated);
                else
                    return Forbid("Admin count is at the threshold");
            }
            catch (Exception err)
            {
                    return Problem(detail: "An error occured in AdminController due to: ", err.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPost("increment-threshold")]
        public IActionResult IncrementAdminThreshold()
        {
            try
            {
                _adminService.IncrementThreshold();
                return Ok();
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,err);
            }
        }
    }
}

