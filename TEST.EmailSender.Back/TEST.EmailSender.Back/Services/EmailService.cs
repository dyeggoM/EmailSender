using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;
using TEST.EmailSender.Back.Entities;
using TEST.EmailSender.Back.Interfaces;

namespace TEST.EmailSender.Back.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// This method sends an email using .Net default SMTP api.
        /// </summary>
        /// <param name="emailDTO">Email parameters.</param>
        /// <returns></returns>
        public async Task<bool> SendTextEmail(EmailDTO emailDTO)
        {
            var t = Task.Run(() => {
                try
                {
                    var smtpClient = new SmtpClient
                    {
                        UseDefaultCredentials = false,
                        Host = _configuration[$"EmailConfiguration:Host"],
                        Port = int.Parse(_configuration[$"EmailConfiguration:Port"]),
                        Credentials = new System.Net.NetworkCredential(_configuration[$"EmailConfiguration:User"], _configuration[$"EmailConfiguration:Password"]),
                        EnableSsl = bool.Parse(_configuration[$"EmailConfiguration:SSL"])
                    };
                    var fromMail = new MailAddress(emailDTO.From, emailDTO.FromName);
                    var toMail = new MailAddress(emailDTO.To);
                    var mail = new MailMessage(fromMail, toMail);
                    mail.Subject = emailDTO.Subject;
                    mail.Body = emailDTO.Body;
                    mail.IsBodyHtml = emailDTO.IsHtml;
                    smtpClient.Send(mail);
                    return true;
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error on {nameof(EmailService)}.{nameof(SendTextEmail)}: {JsonSerializer.Serialize(e)}", e);
                    return false;
                }
            });
            return await t;
        }
    }
}
