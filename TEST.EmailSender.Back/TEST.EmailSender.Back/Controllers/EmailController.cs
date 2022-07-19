using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST.EmailSender.Back.Entities;
using TEST.EmailSender.Back.Interfaces;

namespace TEST.EmailSender.Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IEmailService2 _emailService2;
        public EmailController(IEmailService emailService, IEmailService2 emailService2, IConfiguration configuration)
        {
            _emailService = emailService;
            _emailService2 = emailService2;
            _configuration = configuration;
        }

        /// <summary>
        /// Sends an email using .Net default SMTP api.
        /// </summary>
        /// <param name="email">Email which receives the message.</param>
        /// <response code="200">Returns true or false depending on email send result.</response>
        /// <response code="400">An error occurred during email sending.</response>
        [HttpPost("net-mail")]
        public async Task<IActionResult> NetMail(string email)
        {
            try
            {
                var emailDTO = new EmailDTO()
                {
                    To = email,
                    Body = "<h1>Test Email</h1><p>Net-Mail</p>",
                    IsHtml = true,
                    Subject = "Test Email Net-Mail",
                    From = _configuration["EmailConfiguration:User"]
                };
                var result = await _emailService.SendTextEmail(emailDTO);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Sends an email using MailKit package.
        /// </summary>
        /// <param name="email">Email which receives the message.</param>
        /// <response code="200">Returns true or false depending on email send result.</response>
        /// <response code="400">An error occurred during email sending.</response>
        [HttpPost("mailkit")]
        public async Task<IActionResult> MailKit(string email)
        {
            try
            {
                var emailDTO = new EmailDTO()
                {
                    To = email,
                    Body = "<h1>Test Email</h1><p>MailKit</p>",
                    IsHtml = true,
                    Subject = "Test Email MailKit",
                    From = _configuration["EmailConfiguration:User"]
                };
                var result = await _emailService2.SendTextEmail(emailDTO);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
