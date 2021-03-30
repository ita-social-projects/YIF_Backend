namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class PaymentFormsResponseApiModel
    {
        /// <summary>
        /// Unique id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Get the payment form name for this payment form.
        /// </summary>
        public string Name { get; set; }
    }
}
