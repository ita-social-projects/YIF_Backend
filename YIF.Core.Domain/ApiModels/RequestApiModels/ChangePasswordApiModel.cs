using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class ChangePasswordApiModel
    {
        /// <summary>
        /// User id
        /// </summary>
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string UserId { get; set; }
        /// <summary>
        /// User old password
        /// </summary>
        public string OldPassword { get; set; }
        /// <summary>
        /// User new password
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// Confirm new password
        /// </summary>
        public string ConfirmNewPassword { get; set; }
        /// <summary>
        /// Recaptcha token
        /// </summary>
        public string RecaptchaToken { get; set; }
    }
}
