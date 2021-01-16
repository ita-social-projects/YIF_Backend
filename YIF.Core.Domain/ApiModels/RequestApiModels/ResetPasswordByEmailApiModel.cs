using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class ResetPasswordByEmailApiModel
    {
        [Required]
        public string UserEmail { get; set; }
    }
}
