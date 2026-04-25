using CivicCare.Application.Contracts;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace CivicCare.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(string to, string subject, string body, string? cc = null)
        {
            var smtp = new SmtpClient
            {
                Host = _configuration["Email:Host"],
                Port = int.Parse(_configuration["Email:Port"]),
                Credentials = new NetworkCredential(
                    _configuration["Email:Username"],
                    _configuration["Email:Password"]
                ),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress("no-reply@civiccare.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mail.To.Add(to);

            if (!string.IsNullOrWhiteSpace(cc))
            {
                mail.CC.Add(cc);
            }

            await smtp.SendMailAsync(mail);
        }
    }
}