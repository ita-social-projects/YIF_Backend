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
        private readonly string _senderEmail;
        private readonly string _senderName;

        public SendGridService(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration.GetValue<string>("SendGrid:SendGridApi");
            _senderEmail = _configuration.GetValue<string>("SendGrid:SenderEmail");
            _senderName = _configuration.GetValue<string>("SendGrid:SenderName");
        }
        public async Task<bool> SendAsync(string email, string subject, string content)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_senderEmail, _senderName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

            // disable tracking settings
            // ref.: https://sendgrid.com/docs/User_Guide/Settings/tracking.html

            //msg.SetClickTracking(false, false);
            //msg.SetOpenTracking(false);
            //msg.SetGoogleAnalytics(false);
            //msg.SetSubscriptionTracking(false);

            var response = await client.SendEmailAsync(msg);
            return response.IsSuccessStatusCode;
        }
    }
}
