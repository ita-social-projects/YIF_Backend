﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace YIF.Core.Data.Entities.IdentityEntities
{
    public class DbUser : IdentityUser
    {
        public bool IsDeleted { get; set; } = false;

        public virtual Token Token { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<BaseUser> BaseUsers { get; set; }
        public virtual ICollection<Graduate> Graduates { get; set; }
        public virtual ICollection<InstitutionOfEducationAdmin> InstitutionOfEducationAdmins { get; set; }
        public virtual ICollection<InstitutionOfEducationModerator> InstitutionOfEducationModerators { get; set; }
        public virtual ICollection<Lector> Lectors { get; set; }
        public virtual ICollection<SchoolModerator> SchoolModerators { get; set; } 
        public virtual ICollection<SuperAdmin> SuperAdmins { get; set; } 
    }
}
