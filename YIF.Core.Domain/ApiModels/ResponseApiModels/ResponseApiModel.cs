using System.Net;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    /// <summary>
    /// A сlass for easily create response.
    /// </summary>
    /// <typeparam name="T">A class used in case of a response with returning an object.</typeparam>
    public class ResponseApiModel<T>
    {
        /// <summary>
        /// Gets the value of whether the result is successful (whether the <see cref="HttpStatusCode"/> is 2XX).
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
        public ResponseApiModel(T obj, bool isSuccess = false, string message = null)
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
            Message = message;
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






        //public IActionResult Response()
        //{
        //    switch ((HttpStatusCode)StatusCode)
        //    {
        //        case HttpStatusCode.OK:
        //            if (Object == null)
        //            {
        //                return new OkResult();
        //            }
        //            return new OkObjectResult(Object);
        //        case HttpStatusCode.Created:
        //            return new CreatedResult("", Object);
        //        case HttpStatusCode.Accepted:
        //            return new AcceptedResult();
        //        case HttpStatusCode.NoContent:
        //            return new NoContentResult();
        //        case HttpStatusCode.BadRequest:
        //            if (Message == null)
        //            {
        //                return new BadRequestResult();
        //            }
        //            return new BadRequestObjectResult(Description);
        //        case HttpStatusCode.Unauthorized:
        //            if (Message == null)
        //            {
        //                return new UnauthorizedResult();
        //            }
        //            return new UnauthorizedObjectResult(Description);
        //        case HttpStatusCode.Forbidden:
        //            return new ForbidResult();
        //        case HttpStatusCode.NotFound:
        //            if (Message == null)
        //            {
        //                return new NotFoundResult();
        //            }
        //            return new NotFoundObjectResult(Description);
        //        case HttpStatusCode.Conflict:
        //            if (Message == null)
        //            {
        //                return new ConflictResult();
        //            }
        //            return new ConflictObjectResult(Description);
        //        default:
        //            StatusCode = (int)HttpStatusCode.NotImplemented;
        //            return this;
        //    }
        //}
    }
}
