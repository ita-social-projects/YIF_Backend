using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Data.Entities
{
    public class School : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        /// <summary>
        /// List of school moderators
        /// </summary>
        public virtual ICollection<SchoolModerator> Moderators { get; set; }
        /// <summary>
        /// List of school admins
        /// </summary>
        public virtual ICollection<SchoolAdmin> Admins { get; set; }
        /// <summary>
        /// List of school graduates
        /// </summary>
        public virtual ICollection<Graduate> Graduates { get; set; }
    }
}
