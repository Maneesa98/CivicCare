using CivicCare.Domain.Models;
using System.Net;
using System.Net.Mail;

namespace CivicCare.Application.Contracts
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body, string? cc = null);
        
    }
}
