using System.Text.Json.Serialization;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse
{
    public class PaymentAndEducationFormsPostApiModel
    {
        /// <summary>
        /// Specialty education form
        /// </summary>
        /// <example>Daily</example>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EducationForm EducationForm { get; set; }

        /// <summary>
        /// Specialty payment form
        /// </summary>
        /// <example>Contract</example>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentForm PaymentForm { get; set; }
    }
}
