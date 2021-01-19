using System;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class ErrorDetails
    {
        /// <summary>
        /// Gets or sets the primary key for this error.
        /// </summary>
        public string ErrorId { get; set; }

        /// <summary>
        /// Gets or sets the request path for this error.
        /// </summary>
        public string RequestPath { get; set; }

        /// <summary>
        /// Gets or sets the endpoint path for this error.
        /// </summary>
        public string EndpointPath { get; set; }

        /// <summary>
        /// Gets or sets the time stamp for this error.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the message for this error.
        /// </summary>
        public string Message { get; set; }
    }
}
