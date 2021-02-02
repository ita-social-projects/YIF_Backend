using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF_Backend.Infrastructure.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json; charset=utf-8";

                var desctiption = new DescriptionResponseApiModel(error.Message);
                if (error is BadImageFormatException) desctiption.Message = "Неправильний формат зображення";
                else if (error is FormatException) desctiption.Message = "Неправильний формат даних";
                else if (error is ArgumentNullException) desctiption.Message = "Поле (одне із полів) не може бути пустим";
                var details = new ErrorDetails
                {
                    ErrorId = Guid.NewGuid().ToString(),
                    RequestPath = context.Request.Path.Value,
                    EndpointPath = context.GetEndpoint()?.ToString(),
                    TimeStamp = DateTime.Now,
                    Message = desctiption.Message
                };

                switch (error)
                {
                    case DirectoryNotFoundException _:
                    case FileNotFoundException _:
                    case InvalidOperationException _:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case KeyNotFoundException _:
                    case NotFoundException _:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case ArgumentNullException _:
                    case ArgumentOutOfRangeException _:
                    case BadImageFormatException _:
                    case ArgumentException _:
                    case FormatException _:
                    case BadRequestException _:
                    case SecurityTokenException _:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var jsonOptions = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented
                };
                var result = response.StatusCode == 500
                    ? JsonConvert.SerializeObject(details, jsonOptions)
                    : JsonConvert.SerializeObject(desctiption, jsonOptions);
                await response.WriteAsync(result);
            }
        }
    }
}
