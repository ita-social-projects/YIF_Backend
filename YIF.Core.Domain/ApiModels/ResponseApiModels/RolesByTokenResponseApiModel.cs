using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    /// <summary>
    /// A class for validity of the token and return results with current user roles in response.
    /// </summary>
    public class RolesByTokenResponseApiModel
    {
        /// <summary>
        /// Initializes a new instance result model of validity of the token.
        /// </summary>
        /// <param name="tokenStatus">Sets the token status.</param>
        /// <param name="roles">Sets the roles.</param>
        public RolesByTokenResponseApiModel(string tokenStatus = "Unknown", IEnumerable<string> roles = null)
        {
            TokenStatus = tokenStatus;
            Roles = roles;
        }

        /// <summary>
        /// Gets or sets the validity of the token for response result.
        /// </summary>
        /// <example>Valid</example>
        [Required]
        public string TokenStatus { get; set; }

        /// <summary>
        /// Gets or sets current user roles for response result.
        /// </summary>
        /// <example>[ "admin" ]</example>
        public IEnumerable<string> Roles { get; set; }
    }
}
