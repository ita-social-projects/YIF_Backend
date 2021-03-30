namespace YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse
{
    public class PaymentFormToDescriptionForEditPageResponseApiModel
    {
        /// <summary>
        /// Get the payment form id to which this payment form to description belongs.
        /// </summary>
        public string PaymentFormId { get; set; }
        /// <summary>
        /// Get the payment form name for this payment form to description form.
        /// </summary>
        public string PaymentFormName { get; set; }
    }
}
