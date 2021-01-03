using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class LoginApiModel
    {
        /// <summary>
        /// User email
        /// </summary>     
        /// <example>test333@gmail.com</example>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        /// <example>QWerty-1</example>
        [Required]
        public string Password { get; set; }
    }
}
