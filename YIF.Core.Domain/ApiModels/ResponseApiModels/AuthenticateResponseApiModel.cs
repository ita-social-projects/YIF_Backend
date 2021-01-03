using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    /// <summary>
    /// A class for return authenticate results in response.
    /// </summary>
    public class AuthenticateResponseApiModel
    {
        /// <summary>
        /// Initializes a new instance of authenticate result model.
        /// </summary>
        /// <param name="token">Sets the token.</param>
        /// <param name="refreshToken">Sets the refresh token.</param>
        public AuthenticateResponseApiModel(string token = null, string refreshToken = null)
        {
            Token = token;
            RefreshToken = refreshToken;
        }

        /// <summary>
        /// Gets or sets the token for response result.
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Ijc4ZGZhNWVhLTJkZmUtNDNiNC04OTdhLWJlNjA4NjQ5Yjc1MyIsImVtYWlsIjoidGVzdDMzM0BnbWFpbC5jb20iLCJyb2xlcyI6IkdyYWR1YXRlIiwiZXhwIjoxNjA5MjY4MDk5fQ.1vwS6IIjG4n0h3tOVNzrXYQuj5oa-DgcRXiqbdyd-2U</example>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the refresh token for response result.
        /// </summary>
        /// <example>6NSHvj4KkOPaLGOVgaVBgXN4eGUQtxpaI2R8nSBpczY=</example>
        [Required]
        public string RefreshToken { get; set; }
    }
}
