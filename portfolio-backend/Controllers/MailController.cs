using Microsoft.AspNetCore.Mvc;
using portfolio_backend.Data.Entities;
using portfolio_backend.Services.Interfaces;

namespace portfolio_backend.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<ActionResult<Mail>> SendEmail([FromBody] Mail mail)
        {
            try
            {
                return Ok(await this._mailService.SendMail(mail));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

    }
}
