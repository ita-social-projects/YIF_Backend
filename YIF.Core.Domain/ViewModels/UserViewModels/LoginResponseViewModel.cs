using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.ViewModels
{
    public class LoginResponseViewModel
    {
        public string userToken { get; set; }
        public int statusCode { get; set; }
    }
}
