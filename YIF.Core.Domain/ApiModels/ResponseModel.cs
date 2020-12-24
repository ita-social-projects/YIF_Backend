using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace YIF.Core.Domain.ApiModels
{
    public class ResponseModel<T> : IActionResult
    {
        public int StatusCode { get; set; }
        public bool Success => StatusCode >= 200 || StatusCode <= 226;
        public DescriptionResultApiModel Description { get; set; } = new DescriptionResultApiModel();
        public string Message { get => Description.Message; set => Description.Message = value; }
        public T Object { get; set; }


        public ResponseModel<T> Set(bool isSuccess, string message = null) => Set(isSuccess ? 200 : 400, message);
        public ResponseModel<T> Set(int statusCode, T obj, string message = null)
        {
            StatusCode = statusCode;
            Message = message;
            return this;
        }

        public IActionResult Response(HttpStatusCode code) => Response((int)code);
        public IActionResult Response(int code)
        {
            StatusCode = code;
            return Response();
        }
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
                    if (Object == null)
                    {
                        return new NotFoundResult();
                    }
                    return new NotFoundObjectResult(Object);
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
