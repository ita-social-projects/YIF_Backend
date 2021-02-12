namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    /// <summary>
    /// A class for easily create response.
    /// </summary>
    /// <typeparam name="T">A class used in case of a response with returning an object.</typeparam>
    public class ResponseApiModel<T>
    {
        /// <summary>
        /// Gets the value of whether the result is successful.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Gets or sets the description model for the response.
        /// </summary>
        public DescriptionResponseApiModel Description { get; set; } = new DescriptionResponseApiModel();
        /// <summary>
        /// Gets or sets the message for the <see cref="DescriptionResponseApiModel"/> for the response.
        /// </summary>
        public string Message { get => Description.Message; set => Description.Message = value; }
        /// <summary>
        /// A class used in case of a response with returning an object.
        /// </summary>
        public T Object { get; set; }



        /// <summary>
        /// Initializes a new instance of 'ResponseApiModel'.
        /// </summary>
        /// <param name="isSuccess">The result of work.</param>
        /// <param name="message">The message for the description of the response.</param>
        public ResponseApiModel(bool isSuccess = false, string message = null)
        {
            Success = isSuccess;
            Message = message;
        }
        /// <summary>
        /// Initializes a new instance of 'ResponseApiModel'.
        /// </summary>
        /// <param name="obj">The object used in case of a response with returning an object.</param>
        /// <param name="isSuccess">The result of work.</param>
        /// <param name="message">The message for the description of the response.</param>
        public ResponseApiModel(T obj, bool isSuccess, string message = null)
        {
            Object = obj;
            Success = isSuccess;
            Message = message;
        }



        /// <summary>
        /// Sets properties of the class and returns itself.
        /// </summary>
        /// <param name="isSuccess">The result of work.</param>
        /// <param name="message">The message for the description of the response.</param>
        /// <returns>This response model class.</returns>
        public ResponseApiModel<T> Set(bool isSuccess, string message = null)
        {
            Success = isSuccess;
            if (message != null) Message = message;
            return this;
        }
        /// <summary>
        /// Sets properties of the class and returns itself.
        /// </summary>
        /// <param name="obj">The object used in case of a response with returning an object.</param>
        /// <param name="isSuccess">The result of work.</param>
        /// <param name="message">The message for the description of the response.</param>
        /// <returns>This response model class.</returns>
        public ResponseApiModel<T> Set(T obj, bool isSuccess, string message = null)
        {
            Object = obj;
            return Set(isSuccess, message);
        }
    }
}
