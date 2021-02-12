using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class UniversityModeratorDTO
    {
        public string Id { get; set; }
        public string UniversityId { get; set; }
        public string AdminId { get; set; }

        public UniversityAdminDTO Admin { get; set; }
        public UniversityDTO University { get; set; }
        /// <summary>
        /// Link to Identity user
        /// </summary>
        public UserDTO User { get; set; }
    }
}
