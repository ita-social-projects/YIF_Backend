﻿using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
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
                response.ContentType = "application/json";

                var desctiption = new DescriptionResponseApiModel(error.Message);
                if (error is BadImageFormatException) desctiption.Message = "Неправильний формат зображення";
                if (error is FormatException) desctiption.Message = "Неправильний формат даних";
                if (error is ArgumentNullException) desctiption.Message = "Поле (одне із полів) не може бути пустим";
                var details = new ErrorDetails
                {
                    ErrorId = Guid.NewGuid().ToString(),
                    RequestPath = context.Request.Path.Value,
                    EndpointPath = context.GetEndpoint().ToString(),
                    TimeStamp = DateTime.Now,
                    Message = desctiption.Message
                };

                switch (error)
                {
                    case DirectoryNotFoundException a:
                    case FileNotFoundException b:
                    case InvalidOperationException c:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case KeyNotFoundException a:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case ArgumentNullException a:
                    case ArgumentOutOfRangeException b:
                    case BadImageFormatException c:
                    case ArgumentException d:
                    case FormatException e:
                    case BadRequestException f:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                details.StatusCode = response.StatusCode;

                var result = response.StatusCode == 500 ? JsonSerializer.Serialize(details) : JsonSerializer.Serialize(desctiption);
                await response.WriteAsync(result);
            }
        }
    }
}
