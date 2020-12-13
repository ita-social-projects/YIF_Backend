using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.ViewModels.IdentityViewModels
{
    public class UserViewModel
    {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Role { get; set; }
    }
}
