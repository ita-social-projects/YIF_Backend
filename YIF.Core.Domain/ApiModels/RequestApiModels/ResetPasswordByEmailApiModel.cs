using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class ResetPasswordByEmailApiModel
    {
        /// <summary>
        /// Email of user
        /// </summary>
        /// <example>example@gmail.com</example>
        [Required, StringLength(255)]
        [EmailAddress]
        public string UserEmail { get; set; }
    }
}
