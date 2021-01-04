using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    /// <summary>
    /// A class for validity of the token and return results with current user id in response.
    /// </summary>
    public class IdByTokenResponseApiModel
    {
        /// <summary>
        /// Initializes a new instance result model of validity of the token.
        /// </summary>
        /// <param name="tokenStatus">Sets the token status.</param>
        /// <param name="id">Sets the id.</param>
        public IdByTokenResponseApiModel(string tokenStatus = "Unknown", string id = null)
        {
            TokenStatus = tokenStatus;
            Id = id;
        }

        /// <summary>
        /// Gets or sets the validity of the token for response result.
        /// </summary>
        /// <example>Valid</example>
        [Required]
        public string TokenStatus { get; set; }

        /// <summary>
        /// Gets or sets current user id for response result.
        /// </summary>
        /// <example>[ "admin" ]</example>
        public string Id { get; set; }
    }
}

