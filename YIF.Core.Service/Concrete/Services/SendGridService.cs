using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class SendGridService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _fromMail;

        public SendGridService(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration.GetValue<string>("SendGrid:SendGridApi");
            _fromMail = _configuration.GetValue<string>("SendGrid:SendGridMail");
        }
        public async Task<bool> Send(string email, string subject, string plainTextContent)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromMail, _fromMail);
            subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress(email, email);
            plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return response.IsSuccessStatusCode;
        }
    }
}
