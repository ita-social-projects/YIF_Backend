using System.Collections.Generic;
using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class GraduateDTO
    {
        public string Id { get; set; }
        public string SchoolId { get; set; }

        public SchoolDTO School { get; set; }
        public string UserId { get; set; }

        public UserDTO User { get; set; }
        public ICollection<InstitutionOfEducationToGraduateDTO> InstitutionOfEducationGraduates { get; set; }
    }
}
