using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class SendEmailApproveApiModel
    {
        /// <summary>
        /// User email, where will the letter come 
        /// </summary>
        /// <example>example@gmail.com</example>
        public string UserEmail { get; set; }
    }
}
