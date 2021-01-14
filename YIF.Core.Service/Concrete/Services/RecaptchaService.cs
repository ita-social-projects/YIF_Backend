using Microsoft.Extensions.Configuration;
using YIF.Core.Domain.DtoModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class RecaptchaService : IRecaptchaService
    {
        private readonly IConfiguration _configuration;
        public RecaptchaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsValid(string recaptchaToken)
        {
            var client = new System.Net.WebClient();

            // Get key from appsettings.json
            string privateKey = _configuration.GetValue<string>("Recaptcha:SecretKey");
            string requestComm = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                privateKey, recaptchaToken);

            var googleReply = client.DownloadString(requestComm);

            var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RecaptchaResponseDTO>(googleReply);

            if (captchaResponse.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
