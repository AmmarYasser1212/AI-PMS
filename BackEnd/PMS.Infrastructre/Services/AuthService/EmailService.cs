using Microsoft.Extensions.Configuration;
using PMS.Application.Interfaces.Services;
using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Infrastructre.Services.AuthService
{
    public class EmailService :IEmailServices
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            
            _configuration = configuration;
        }

        public async Task EmailSendAsync(string toEmail, string subject, string body)
        {
            var from = _configuration["EmailSettings:Email"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]!);
            var username=_configuration["EmailSettings:SenderName"];
            var password=_configuration["EmailSettings:AppPassword"];

            var message= new MailMessage(from!, toEmail,subject,body);
            message.IsBodyHtml = true;
            using var client = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(from, password),
                EnableSsl = true
            };
            await client.SendMailAsync(message);
        }
    }
}
