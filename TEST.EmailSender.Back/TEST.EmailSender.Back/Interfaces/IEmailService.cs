using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST.EmailSender.Back.Entities;

namespace TEST.EmailSender.Back.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// This method sends an email using .Net default SMTP api.
        /// </summary>
        /// <param name="emailDTO">Email parameters.</param>
        /// <returns></returns>
        Task<bool> SendTextEmail(EmailDTO emailDTO);
    }
}
