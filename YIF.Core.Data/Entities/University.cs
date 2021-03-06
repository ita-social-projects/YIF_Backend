using System;
using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public class University : BaseEntity
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Site { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public DateTime StartOfCampaign { get; set; }
        public DateTime EndOfCampaign { get; set; }

        /// <summary>
        /// List of university admins
        /// </summary>
        public virtual ICollection<UniversityAdmin> Admins { get; set; }
        /// <summary>
        /// List of university moderators
        /// </summary>
        public virtual ICollection<UniversityModerator> Moderators { get; set; }
        /// <summary>
        /// List of university lectures
        /// </summary>
        public virtual ICollection<Lecture> Lectures { get; set; }
        /// <summary>
        /// List of graduates who liked the university
        /// </summary>
        public virtual ICollection<UniversityToGraduate> UniversityGraduates { get; set; }
        public virtual ICollection<SpecialtyToUniversityToGraduate> SpecialtyToUniversityToGraduates { get; set; }

    }
}
