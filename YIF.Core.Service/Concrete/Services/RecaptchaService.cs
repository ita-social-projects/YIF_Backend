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
            string PrivateKey = _configuration.GetValue<string>("Recaptcha:SecretKey");
            string requestComm = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                PrivateKey, recaptchaToken);

            var GoogleReply = client.DownloadString(requestComm);

            var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RecaptchaResponseDTO>(GoogleReply);

            if (captchaResponse.Success)
            {
                return true;
            }
            else
            {
                //throw new Exception("Виникла помилка при підтвердженні каптчи.");
                return false;
            }
        }
    }
}
