using System;
using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.IdentityDTO
{
    public class UserDTO
    {
        /// <summary>
        /// Gets or sets the primary key for this user.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        public string NormalizedUserName { get; set; }
        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        public string NormalizedEmail { get; set; }
        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        public bool EmailConfirmed { get; set; }
        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        public string PasswordHash { get; set; }
        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed).
        /// </summary>
        public string SecurityStamp { get; set; }
        /// <summary>
        /// A random value that must change whenever a user is persisted to the store.
        /// </summary>
        public string ConcurrencyStamp { get; set; }
        /// <summary>
        /// Gets or sets a telephone number for the user.
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their telephone address.
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }
        /// <summary>
        /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
        /// </summary>
        public bool TwoFactorEnabled { get; set; }
        /// <summary>
        /// Gets or sets the date and time, in UTC, when any user lockout ends.
        /// </summary>
        public DateTimeOffset? LockoutEnd { get; set; }
        /// <summary>
        /// Gets or sets a flag indicating if the user could be locked out.
        /// </summary>
        public bool LockoutEnabled { get; set; }
        /// <summary>
        /// Gets or sets the number of failed login attempts for the current user.
        /// </summary>
        public int AccessFailedCount { get; set; }
        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual IEnumerable<string> Roles { get; set; }
    }
}
