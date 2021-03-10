using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class UniversityModeratorDTO
    {
        public string Id { get; set; }
        
        public string AdminId { get; set; }
        public virtual UniversityAdminDTO Admin { get; set; }

        public string UserId { get; set; }
        public UserDTO User { get; set; }
    }
}
