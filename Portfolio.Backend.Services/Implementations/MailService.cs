using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Portfolio.Backend.Services.Interfaces;
using Portfolio.Backend.Common.Data.Entities;

namespace Portfolio.Backend.Services.Implementations
{

    public class MailService : IMailService
    {
        private readonly IConfiguration configuration;
        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<Mail> SendMail(Mail mail)
        {
            MailAddress to = new(mail.Destination);
            MailAddress from = new("noreply@serdilcakmak.com");
            MailMessage msg = new(from, to);
            msg.Subject = mail.Subject;
            msg.Body = "<h1>Sent by: " + mail.Sender + "</h1><br/>" + "<pre>" + mail.Message + "</pre>";
            msg.IsBodyHtml = true;
            var auth = configuration.GetSection("EmailConfig").GetChildren().ToList();
            var id = auth[0].Value;
            var password = auth[1].Value;
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential(id, password),
                EnableSsl = true
            };
            await client.SendMailAsync(msg);
            msg.Dispose();
            client.Dispose();
            return mail;
        }
    }
}