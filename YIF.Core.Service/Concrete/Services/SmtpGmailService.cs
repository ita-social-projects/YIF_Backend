using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class SmtpGmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly string _senderPassword;

        public SmtpGmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _senderEmail = _configuration.GetValue<string>("SmtpGmail:SenderEmail");
            _senderName = _configuration.GetValue<string>("SmtpGmail:SenderName");
            _senderPassword = _configuration.GetValue<string>("SmtpGmail:SenderPassword");
        }

        public async Task<bool> SendAsync(string email, string subject, string content)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(_senderEmail, _senderName);
            mail.To.Add(email);
            mail.Subject = subject;
            mail.IsBodyHtml = true;

            mail.Body = content;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(_senderEmail, _senderPassword),
                EnableSsl = true
            };

            try
            {
                await client.SendMailAsync(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
