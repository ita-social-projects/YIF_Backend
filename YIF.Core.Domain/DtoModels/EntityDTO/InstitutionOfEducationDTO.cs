using System;
using System.Collections.Generic;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class InstitutionOfEducationDTO
    {
        public string Id { get; set; }
        /// <summary>
        /// Name of institutionOfEducation
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Short name of institutionOfEducation
        /// </summary>
        public string Abbreviation { get; set; }
        /// <summary>
        /// Site of institutionOfEducation
        /// </summary>
        public string Site { get; set; }
        /// <summary>
        /// Address of institutionOfEducation
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Phone of institutionOfEducation
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// AdminEmail of institutionOfEducation
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Description of institutionOfEducation
        /// </summary> 
        public string Description { get; set; }
        /// <summary>
        /// Path of the institution Of Education image in project directory
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// Latitude of institutio nOf Education
        /// </summary>
        public float Lat { get; set; }
        /// <summary>
        /// Longitude of institution Of Education
        /// </summary>
        public float Lon { get; set; }
        /// <summary>
        /// Type of institution Of Education
        /// </summary>
        public InstitutionOfEducationType InstitutionOfEducationType { get; set; }
        /// <summary>
        /// Start date of the entrance campaign
        /// </summary>
        public DateTime StartOfCampaign { get; set; }
        /// <summary>
        /// End date of the entrance campaign
        /// </summary>
        public DateTime EndOfCampaign { get; set; }
        /// <summary>
        /// Is the institution Of Education favorite
        /// </summary>
        public bool IsFavorite { get; set; }

        /// <summary>
        /// List of institution Of Education admins
        /// </summary>
        public virtual IEnumerable<InstitutionOfEducationAdminDTO> Admins { get; set; }
        /// <summary>
        /// List of institution Of Education lectures
        /// </summary>
        public virtual IEnumerable<LectureDTO> Lectures { get; set; }
        /// <summary>
        /// List of graduates who liked the institution Of Education
        /// </summary>
        public virtual ICollection<InstitutionOfEducationToGraduateDTO> InstitutionOfEducationGraduates { get; set; }
        public virtual ICollection<DirectionToInstitutionOfEducationDTO> DirectionToInstitutionOfEducation { get; set; }
    }
}
