using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// Description of university
        /// </summary> 
        public string Description { get; set; }
        /// <summary>
        /// Path of the university image in project directory
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// List of university admins
        /// </summary>
        public virtual IEnumerable<UniversityAdminDTO> Admins { get; set; }
        /// <summary>
        /// List of university moderators
        /// </summary>
        public virtual IEnumerable<UniversityModeratorDTO> Moderators { get; set; }
        /// <summary>
        /// List of university lectures
        /// </summary>
        public virtual IEnumerable<LectureDTO> Lectures { get; set; }
    }
}
