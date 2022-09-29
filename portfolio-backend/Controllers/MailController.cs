using System;
using System.Threading.Tasks;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IMailService mailService;

        public MailController(IMailService mailService_)
        {
            this.mailService = mailService_;
        }

        [HttpPost]
        public async Task<ActionResult<Mail>> SendEmail([FromBody] Mail mail)
        {
            try
            {
                return Ok(await this.mailService.SendMail(mail));
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

    }
}
