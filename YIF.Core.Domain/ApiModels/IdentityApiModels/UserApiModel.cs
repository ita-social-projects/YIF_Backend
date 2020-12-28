using System.Collections.Generic;

namespace YIF.Core.Domain.ApiModels.IdentityApiModels
{
    public class UserApiModel
    {
        /// <summary>
        /// Gets or sets the primary key for this user.
        /// </summary>
        /// <example>01f75261-2feb-4a34-93fb-ab26bf16cbe7</example>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        /// <example>Moderator228</example>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        /// <example>anataly@gmail.com</example>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a telephone number for the user.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual IEnumerable<string> Roles { get; set; }
    }
}
