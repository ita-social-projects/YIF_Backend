namespace YIF.Core.Domain.ApiModels
{
    /// <summary>
    /// A class for return result as the description.
    /// </summary>
    /// <remarks>
    /// Usually used the error or the describe message.
    /// </remarks>
    public class DescriptionResultApiModel
    {
        /// <summary>
        /// Initializes a new instance of description result.
        /// </summary>
        /// <param name="message">Sets the message to describe</param>
        public DescriptionResultApiModel(string message = null)
        {
            Message = message;
        }
        /// <summary>
        /// Gets or sets the message for response description.
        /// </summary>
        public string Message { get; set; }
    }
}
