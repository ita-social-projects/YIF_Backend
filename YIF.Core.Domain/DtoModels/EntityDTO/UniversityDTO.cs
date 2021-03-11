using System;
using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class UniversityDTO
    {
        public string Id { get; set; }
        /// <summary>
        /// Name of university
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Short name of university
        /// </summary>
        public string Abbreviation { get; set; }
        /// <summary>
        /// Site of university
        /// </summary>
        public string Site { get; set; }
        /// <summary>
        /// Address of university
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Phone of university
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Email of university
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Description of university
        /// </summary> 
        public string Description { get; set; }
        /// <summary>
        /// Path of the university image in project directory
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// Latitude of university
        /// </summary>
        public float Lat { get; set; }
        /// <summary>
        /// Longitude of university
        /// </summary>
        public float Lon { get; set; }
        /// <summary>
        /// Start date of the entrance campaign
        /// </summary>
        public DateTime StartOfCampaign { get; set; }
        /// <summary>
        /// End date of the entrance campaign
        /// </summary>
        public DateTime EndOfCampaign { get; set; }
        /// <summary>
        /// Is the university favorite
        /// </summary>
        public bool IsFavorite { get; set; }

        /// <summary>
        /// List of university admins
        /// </summary>
        public virtual IEnumerable<UniversityAdminDTO> Admins { get; set; }
        /// <summary>
        /// List of university lectures
        /// </summary>
        public virtual IEnumerable<LectureDTO> Lectures { get; set; }
        /// <summary>
        /// List of graduates who liked the university
        /// </summary>
        public virtual ICollection<UniversityToGraduateDTO> UniversityGraduates { get; set; }
    }
}
