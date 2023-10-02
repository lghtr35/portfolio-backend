using Portfolio.Backend.Common.Data.Entities;

namespace Portfolio.Backend.Services.Interfaces
{
    public interface IMailService
    {
        Task<Mail> SendMail(Mail mail);
    }
}