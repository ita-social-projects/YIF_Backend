using System;
using System.Collections.Generic;
using System.Text;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.DtoModels
{
    class UniversityDTO
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
        public virtual ICollection<UniversityAdminDTO> Admins { get; set; }
        /// <summary>
        /// List of university moderators
        /// </summary>
        public virtual ICollection<UniversityModeratorDTO> Moderators { get; set; }
        /// <summary>
        /// List of university lectures
        /// </summary>
        public virtual ICollection<LectureDTO> Lectures { get; set; }
    }
}
