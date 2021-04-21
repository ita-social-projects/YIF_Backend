using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse
{
    public class PaymentAndEducationFormsResponseApiModel
    {
        /// <summary>
        /// Institution Of Education form
        /// </summary>
        /// <example>Daily</example>
        public EducationForm educationForm { get; set; }

        /// <summary>
        /// Institution Of Education payment form
        /// </summary>
        /// <example>Contract</example>
        public PaymentForm paymentForm { get; set; }
    }
}
