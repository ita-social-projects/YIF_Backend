using Microsoft.AspNetCore.Identity;

namespace YIF.Core.Data.Entities.IdentityEntities
{
    public class DbUser : IdentityUser
    {
        public virtual Token Token { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public bool IsDeleted { get; set; } = false;
=========
        public virtual UserProfile UserProfile { get; set; }
>>>>>>>>> Temporary merge branch 2
    }
}
