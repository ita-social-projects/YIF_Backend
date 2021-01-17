using System;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    /// <summary>
    /// A class for sending user profile information.
    /// </summary>
    public class UserProfileShortApiModel
    {
        /// <summary>
        /// Get or set the user surname
        /// </summary>
        /// <example>Іванов</example>
        public string Surname { get; set; }

        /// <summary>
        /// Get or set the user name
        /// </summary>
        /// <example>Іван</example>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the user middle name
        /// </summary>
        /// <example>Іванович</example>
        public string MiddleName { get; set; }

        /// <summary>
        /// Get or set the user email.
        /// </summary>
        /// <example>example@gmail.com</example>
        public string Email { get; set; }

        /// <summary>
        /// Get or set the user phone number.
        /// </summary>
        /// <example>+380672222000</example>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Get or set the school name to which this user belongs.
        /// </summary>
        /// <example>SomeName</example>
        public string SchoolName { get; set; }
    }
}
