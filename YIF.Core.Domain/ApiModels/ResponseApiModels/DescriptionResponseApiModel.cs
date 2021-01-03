namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    /// <summary>
    /// A class for return the description in response.
    /// </summary>
    /// <remarks>
    /// Usually used the error or the describe message.
    /// </remarks>
    public class DescriptionResponseApiModel
    {
        /// <summary>
        /// Initializes a new instance of description result.
        /// </summary>
        /// <param name="message">Sets the message to describe.</param>
        public DescriptionResponseApiModel(string message = null)
        {
            Message = message;
        }

        /// <summary>
        /// Gets or sets the message for response description.
        /// </summary>
        public string Message { get; set; }
    }
}
