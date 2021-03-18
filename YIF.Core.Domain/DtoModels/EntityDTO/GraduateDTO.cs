using System.Collections.Generic;
using YIF.Core.Domain.DtoModels.IdentityDTO;

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
        /// List of favorite institutionOfEducations
        /// </summary>
        public virtual ICollection<InstitutionOfEducationToGraduateDTO> InstitutionOfEducationGraduates { get; set; }
    }
}
