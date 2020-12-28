using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class RegisterApiModel
    {
        /// <summary>
        /// User email address. Used for login.
        /// </summary>     
        /// <example>test333@gmail.com</example>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Unique username.
        /// </summary>     
        /// <example>TestName</example>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// User password. Must have at least one non alphanumeric character.
        /// 
        /// </summary>     
        /// <example>QWerty-1</example>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Repeat user password. Must match password.
        /// </summary>     
        /// <example>QWerty-1</example>
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
