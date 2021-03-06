﻿namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    /// <summary>
    /// A class for create a response with returning a redirect link.
    /// </summary>
    public class RedirectResponseApiModel
    {
        /// <summary>
        /// Gets or sets the link for redirect.
        /// </summary>
        /// <example>https://yifbackend.tk/api/Redirect</example>
        public string RedirectLink { get; set; }
        /// <summary>
        /// Gets or sets the message for the response.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of 'RedirectApiModel'.
        /// </summary>
        /// <param name="link">The link for redirect.</param>
        /// <param name="message">The message for the description of the reason to redirect.</param>
        public RedirectResponseApiModel(string link, string message = null)
        {
            RedirectLink = link;
            Message = message;
        }
    }
}
