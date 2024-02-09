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
            var fromAdress = new MailAddress(configuration.GetSection("EmailConfig").GetValue<string>("address") ?? "", "Serdil Cakmak");
            var toAddress = new MailAddress(configuration.GetSection("EmailConfig").GetValue<string>("destination") ?? "", "Serdıl Çakmak");
            MailMessage msg = new(fromAdress, toAddress)
            {
                Subject = mail.Subject,
                Body = "<pre>" + mail.Message + "</pre>" + "<br/>" + "<h2>Sent by: " + mail.Sender + "</h2>",
                IsBodyHtml = true
            };
            var auth = configuration.GetSection("EmailConfig");
            var client = new SmtpClient(auth["server"], auth.GetValue<int>("port"))
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(auth["id"], auth["password"]),
                EnableSsl = true
            };
            await client.SendMailAsync(msg);
            msg.Dispose();
            client.Dispose();
            return mail;
        }
    }
}