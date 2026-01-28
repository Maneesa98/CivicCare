using System.Net;
using System.Net.Mail;

namespace CivicCare.Api.Services
{
    public class EmailService
    {
        public void Send(string to, string subject, string body)
        {
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("user", "pass"),
                EnableSsl = true
            };
            client.Send("noreply@civiccare.com", to, subject, body);
        }
    }
}
