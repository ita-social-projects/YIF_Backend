using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.IdentityApiModels
{
    /// <summary>
    /// A class for sending user profile information.
    /// </summary>
    public class UserProfileWithoutPhotoApiModel
    {

        /// <summary>
        /// Get or set the user name
        /// </summary>
        /// <example>Іван</example>
        [StringLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Get or set the user surname
        /// </summary>
        /// <example>Іванов</example>
        [StringLength(255)]
        public string Surname { get; set; }

        /// <summary>
        /// Get or set the user middle name
        /// </summary>
        /// <example>Іванович</example>
        [StringLength(255)]
        public string MiddleName { get; set; }

        /// <summary>
        /// Get or set the user email.
        /// </summary>
        /// <example>example@gmail.com</example>
        [StringLength(255)]
        public string Email { get; set; }

        /// <summary>
        /// Get or set the user phone number.
        /// </summary>
        /// <example>+380672222000</example>
        [Phone]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Get or set the school name to which this user belongs.
        /// </summary>        
        [Required]
        public string SchoolName { get; set; }
    }
}
