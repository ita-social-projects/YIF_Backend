using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.ApiModels.ResultApiModels
{
    public class RegisterApiModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
