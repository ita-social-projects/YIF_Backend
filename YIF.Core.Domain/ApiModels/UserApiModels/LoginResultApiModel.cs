namespace YIF.Core.Domain.ApiModels.UserApiModels
{
    /// <summary>
    /// A class for return login results.
    /// </summary>
    public class LoginResultApiModel
    {
        /// <summary>
        /// Initializes a new instance of login result model.
        /// </summary>
        /// <param name="token">Sets the token</param>
        public LoginResultApiModel(string token = null)
        {
            Token = token;
        }
        /// <summary>
        /// Gets or sets the token for response result.
        /// </summary>
        public string Token { get; set; }
    }
}
