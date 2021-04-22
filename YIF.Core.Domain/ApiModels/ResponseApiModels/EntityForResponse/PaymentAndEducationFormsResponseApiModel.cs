using System.Text.Json.Serialization;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse
{
    public class PaymentAndEducationFormsResponseApiModel
    {
        /// <summary>
        /// Institution Of Education form
        /// </summary>
        /// <example>Daily</example>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EducationForm EducationForm { get; set; }

        /// <summary>
        /// Institution Of Education payment form
        /// </summary>
        /// <example>Contract</example>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentForm PaymentForm { get; set; }
    }
}
