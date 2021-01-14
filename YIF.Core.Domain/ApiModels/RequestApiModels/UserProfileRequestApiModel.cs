using System;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    /// <summary>
    /// A class for sending user profile information.
    /// </summary>
    public class UserProfileRequestApiModel
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
        /// Get or set the username.
        /// </summary>
        /// <example>SomeName</example>
        public string School { get; set; }

        /// <summary>
        /// Get or set user date of birth
        /// </summary>
        /// <example>2021-01-13T21:22:08.836Z</example>
        public DateTime? DateOfBirth { get; set; }
    }
}
