using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Data.Entities
{
    public class University : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        /// <summary>
        /// List of university specialities
        /// </summary>
        //public ICollection<Speciality> Specialities { get; set; }

        /// <summary>
        /// List of university admins
        /// </summary>
        public ICollection<UniversityAdmin> Admins { get; set; }
        /// <summary>
        /// List of university moderators
        /// </summary>
        public ICollection<UniversityModerator> Moderators { get; set; }
        /// <summary>
        /// List of university lectures
        /// </summary>
        public ICollection<Lecture> Lectures { get; set; }
    }
}
