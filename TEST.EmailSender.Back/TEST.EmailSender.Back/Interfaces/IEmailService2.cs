using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST.EmailSender.Back.Entities;

namespace TEST.EmailSender.Back.Interfaces
{
    public interface IEmailService2
    {
        /// <summary>
        /// This method sends an email using MailKit package, using email credentials to sign in SMTP client.
        /// </summary>
        /// <param name="emailDTO">Email parameters.</param>
        /// <returns></returns>
        Task<bool> SendTextEmail(EmailDTO emailDTO);
    }
}
