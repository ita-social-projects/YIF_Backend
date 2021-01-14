using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.DtoModels.IdentityDTO
{
    public class UserProfileDTO
    {
        /// <summary>
        /// Get or set user id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Get or set the username.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Get or set the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get or set the phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Get or set the user name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the user middle name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Get or set the user surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Get or set user photo
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Get or set user date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Get or set registration date
        /// </summary>
        public DateTime RegistrationDate { get; set; }
    }
}
