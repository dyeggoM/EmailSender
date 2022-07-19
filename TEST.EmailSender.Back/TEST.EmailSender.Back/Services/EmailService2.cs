using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TEST.EmailSender.Back.Entities;
using TEST.EmailSender.Back.Interfaces;

namespace TEST.EmailSender.Back.Services
{
    public class EmailService2 : IEmailService2
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService2> _logger;
        public EmailService2(IConfiguration configuration, ILogger<EmailService2> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// This method sends an email using MailKit package, using email credentials to sign in SMTP client.
        /// </summary>
        /// <param name="emailDTO">Email parameters.</param>
        /// <returns></returns>
        public async Task<bool> SendTextEmail(EmailDTO emailDTO)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(emailDTO.FromName, emailDTO.From));
                message.To.Add(new MailboxAddress("", emailDTO.To));
                message.Bcc.Add(new MailboxAddress("", emailDTO.From));
                message.Subject = emailDTO.Subject;
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = emailDTO.Body
                };
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_configuration[$"EmailConfiguration:Host"], int.Parse(_configuration[$"EmailConfiguration:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration[$"EmailConfiguration:User"], _configuration[$"EmailConfiguration:Password"]);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error on {nameof(EmailService2)}.{nameof(SendTextEmail)}: {JsonSerializer.Serialize(e)}", e);
                return false;
            }
        }

    }
}
