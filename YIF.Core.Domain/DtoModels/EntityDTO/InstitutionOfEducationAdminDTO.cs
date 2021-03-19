using System.Collections.Generic;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class InstitutionOfEducationAdminDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public virtual UserDTO User { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public virtual InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
        public virtual ICollection<InstitutionOfEducationModeratorDTO> Moderators { get; set; }
        public bool IsBanned { get; set; }
    }
}
