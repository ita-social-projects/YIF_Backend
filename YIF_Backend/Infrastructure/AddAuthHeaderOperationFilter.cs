using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace YIF_Backend.Infrastructure
{
    public class AddAuthorizationHeaderOperationHeader : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var actionMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            var isAuthorized = actionMetadata.Any(metadataItem => metadataItem is AuthorizeAttribute);
            var allowAnonymous = actionMetadata.Any(metadataItem => metadataItem is AllowAnonymousAttribute);

            if (!isAuthorized || allowAnonymous)
            {
                return;
            }
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            if (operation.Responses.ContainsKey("401")) operation.Responses.Remove("401");
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            if (operation.Responses.ContainsKey("403")) operation.Responses.Remove("403");
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            //Add JWT bearer type
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new string[0]
                    }
                }
            };
        }
    }
}
