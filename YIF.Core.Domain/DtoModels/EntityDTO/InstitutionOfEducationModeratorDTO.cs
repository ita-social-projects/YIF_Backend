using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class InstitutionOfEducationModeratorDTO
    {
        public string Id { get; set; }
        
        public string AdminId { get; set; }
        public InstitutionOfEducationAdminDTO Admin { get; set; }

        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public bool IsBanned { get; set; } = false;
    }
}
