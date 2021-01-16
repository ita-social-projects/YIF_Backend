using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class ResetPasswordByEmailApiModel
    {
        /// <summary>
        /// Email of user
        /// </summary>
        /// <example>example@gmail.com</example>
        [Required, StringLength(255)]
        public string UserEmail { get; set; }
    }
}
