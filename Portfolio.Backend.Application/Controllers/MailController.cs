using Microsoft.AspNetCore.Mvc;
using Portfolio.Backend.Common.Data.Entities;
using Portfolio.Backend.Services.Interfaces;

// TODO: Better error handling and logging
namespace Portfolio.Backend.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly ILogger _logger;

        public MailController(IMailService mailService, ILogger<MailController> logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Mail>> SendEmail([FromBody] Mail mail)
        {
            try
            {
                _logger.LogInformation("Send Mail recieved a request");
                return Ok(await _mailService.SendMail(mail));
            }
            catch (Exception err)
            {
                _logger.LogError("An error occured in MailController due to: " + err.Message, err);
                return StatusCode(StatusCodes.Status500InternalServerError, err.ToString());
            }
        }

    }
}
