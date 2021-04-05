using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public class School : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public ICollection<SchoolModerator> Moderators { get; set; }
        public ICollection<SchoolAdmin> Admins { get; set; }
        public ICollection<Graduate> Graduates { get; set; }
    }
}
