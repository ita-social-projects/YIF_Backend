using System.Collections.Generic;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class UniversityAdminDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public virtual UserDTO User { get; set; }
        public string UniversityId { get; set; }
        public virtual UniversityDTO University { get; set; }
        public virtual ICollection<UniversityModeratorDTO> Moderators { get; set; }
        public bool IsBanned { get; set; }
    }
}
