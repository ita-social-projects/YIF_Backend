using System.Collections.Generic;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.DtoModels.School;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class GraduateDTO
    {
        public string Id { get; set; }
        public string SchoolId { get; set; }

        public virtual SchoolDTO School { get; set; }
        /// <summary>
        /// Link to Identity user
        /// </summary>
        public string UserId { get; set; }

        public virtual UserDTO User { get; set; }
        /// <summary>
        /// List of favorite universities
        /// </summary>
        public virtual ICollection<UniversityToGraduateDTO> UniversityGraduates { get; set; }
    }
}
