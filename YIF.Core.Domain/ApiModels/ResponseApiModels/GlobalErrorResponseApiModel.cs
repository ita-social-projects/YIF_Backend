namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class GlobalErrorResponseApiModel
    {
        /// <summary>
        /// Gets or sets the error description model for the response.
        /// </summary>
        public ErrorDetails Details { get; set; } = new ErrorDetails();
    }
}
