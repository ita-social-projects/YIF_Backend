using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Data.Entities
{
    public class DbRole : IdentityRole<string>
    {
        public virtual ICollection<DbUserRole> UserRoles { get; set; }
    }
}
