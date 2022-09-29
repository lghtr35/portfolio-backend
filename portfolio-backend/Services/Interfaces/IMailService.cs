using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.Extensions.Configuration;

namespace backend.Services
{
    public interface IMailService
    {
        Task<Mail> SendMail(Mail mail);
    }
}