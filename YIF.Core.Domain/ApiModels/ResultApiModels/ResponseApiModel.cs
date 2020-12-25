using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace YIF.Core.Domain.ApiModels.ResultApiModels
{
    /// <summary>
    /// A сlass for easily create response.
    /// </summary>
    /// <typeparam name="T">A class used in case of a response with returning an object.</typeparam>
    public class ResponseApiModel<T> : IActionResult
    {
        /// <summary>
        /// Gets or sets the status code for the response.
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Gets the value of whether the result is successful (whether the <see cref="HttpStatusCode"/> is 2XX).
        /// </summary>
        public bool Success => StatusCode >= 200 && StatusCode <= 226;
        /// <summary>
        /// Gets or sets the description model for the response.
        /// </summary>
        public DescriptionResultApiModel Description { get; set; } = new DescriptionResultApiModel();
        /// <summary>
        /// Gets or sets the message for the <see cref="DescriptionResultApiModel"/> for the response.
        /// </summary>
        public string Message { get => Description.Message; set => Description.Message = value; }
        /// <summary>
        /// A class used in case of a response with returning an object.
        /// </summary>
        public T Object { get; set; }


        /// <summary>
        /// Sets properties of the class.
        /// </summary>
        /// <param name="isSuccess">Is the result successfully? Returns the <see cref="OkResult"/> with code 200, if true,
        /// and <see cref="BadRequestResult"/> result with code 400, if false.</param>
        /// <param name="message">The message for the description of the response.</param>
        /// <returns>This response model class.</returns>
        public ResponseApiModel<T> Set(bool isSuccess, string message = null) => Set(isSuccess ? 200 : 400, message);
        /// <summary>
        /// Sets properties of the class.
        /// </summary>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> during response.</param>
        /// <param name="message">The message for the description of the response.</param>
        /// <returns>This response model class.</returns>
        public ResponseApiModel<T> Set(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message;
            return this;
        }
        /// <summary>
        /// Sets properties of the class.
        /// </summary>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> during response.</param>
        /// <param name="obj">The object used in case of a response with returning an object.</param>
        /// <param name="message">The message for the description of the response.</param>
        /// <returns>This response model class.</returns>
        public ResponseApiModel<T> Set(int statusCode, T obj, string message = null)
        {
            Object = obj;
            return Set(statusCode, message);
        }

        /// <summary>
        /// Creates response.
        /// </summary>
        /// <param name="code">The <see cref="HttpStatusCode"/> during response.</param>
        /// <returns><see cref="IActionResult"/> object.</returns>
        public IActionResult Response(HttpStatusCode code) => Response((int)code);
        /// <summary>
        /// Creates response.
        /// </summary>
        /// <param name="code">The <see cref="HttpStatusCode"/> as <see cref="int"/> during response.</param>
        /// <returns><see cref="IActionResult"/> object.</returns>
        public IActionResult Response(int code)
        {
            StatusCode = code;
            return Response();
        }
        /// <summary>
        /// Creates response with automatic type detection.
        /// </summary>
        /// <returns><see cref="IActionResult"/> object with automatic response type detection.</returns>
        public IActionResult Response()
        {
            switch ((HttpStatusCode)StatusCode)
            {
                case HttpStatusCode.OK:
                    if (Object == null)
                    {
                        return new OkResult();
                    }
                    return new OkObjectResult(Object);
                case HttpStatusCode.Created:
                    return new CreatedResult("", Object);
                case HttpStatusCode.Accepted:
                    return new AcceptedResult();
                case HttpStatusCode.NoContent:
                    return new NoContentResult();
                case HttpStatusCode.BadRequest:
                    if (Description == null)
                    {
                        return new BadRequestResult();
                    }
                    return new BadRequestObjectResult(Description);
                case HttpStatusCode.Unauthorized:
                    if (Description == null)
                    {
                        return new UnauthorizedResult();
                    }
                    return new UnauthorizedObjectResult(Description);
                case HttpStatusCode.Forbidden:
                    return new ForbidResult();
                case HttpStatusCode.NotFound:
                    if (Description == null)
                    {
                        return new NotFoundResult();
                    }
                    return new NotFoundObjectResult(Description);
                case HttpStatusCode.Conflict:
                    if (Description == null)
                    {
                        return new ConflictResult();
                    }
                    return new ConflictObjectResult(Description);
                default:
                    StatusCode = (int)HttpStatusCode.NotImplemented;
                    return this;
            }
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCode;
            return Task.FromResult(this);
        }
    }
}
