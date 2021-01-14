using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    /// <summary>
    /// A class for returning information about user.
    /// </summary>
    public class UserProfileApiModel
    {
        /// <summary>
        /// Get or set user id.
        /// </summary>
        /// <example>069e4ae0-3cae-4761-b312-7f970e756d47</example>
        public string Id { get; set; }

        /// <summary>
        /// Get or set the username.
        /// </summary>
        /// <example>SomeName</example>
        public string UserName { get; set; }

        /// <summary>
        /// Get or set the email.
        /// </summary>
        /// <example>example@gmail.com</example>
        public string Email { get; set; }

        /// <summary>
        /// Get or set the phone number.
        /// </summary>
        /// <example>+380672222000</example>
        public string PhoneNumber { get; set; }

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
        /// Get or set the user surname
        /// </summary>
        /// <example>Іванов</example>
        public string Surname { get; set; }

        /// <summary>
        /// Get or set user photo
        /// </summary>
        /// <example>069e4ae0-3cae-4761-b312-7f970e756d47</example>
        public string Photo { get; set; }

        /// <summary>
        /// Get or set user date of birth
        /// </summary>
        /// <example>2021-01-13T21:22:08.836Z</example>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Get or set registration date
        /// </summary>
        /// <example>2021-01-13T21:22:08.836Z</example>
        public DateTime RegistrationDate { get; set; }
    }
}
