using portfolio_backend.Data.Entities;

namespace portfolio_backend.Services.Interfaces
{
    public interface IMailService
    {
        Task<Mail> SendMail(Mail mail);
    }
}