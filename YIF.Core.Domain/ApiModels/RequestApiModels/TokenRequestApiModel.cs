namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class TokenRequestApiModel
    {
        /// <summary>
        /// A token containing user ID, email, and roles.
        /// </summary>     
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Ijc4ZGZhNWVhLTJkZmUtNDNiNC04OTdhLWJlNjA4NjQ5Yjc1MyIsImVtYWlsIjoidGVzdDMzM0BnbWFpbC5jb20iLCJyb2xlcyI6IkdyYWR1YXRlIiwiZXhwIjoxNjA5MjY4MDk5fQ.1vwS6IIjG4n0h3tOVNzrXYQuj5oa-DgcRXiqbdyd-2U</example>
        public string Token { get; set; }

        /// <summary>
        /// Disposable refresh token.
        /// </summary>     
        /// <example>6NSHvj4KkOPaLGOVgaVBgXN4eGUQtxpaI2R8nSBpczY=</example>
        public string RefreshToken { get; set; }
    }
}