using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class UniversityAdminDTO
    {
        public string Id { get; set; }
        public UserDTO User { get; set; }
        public UniversityDTO University { get; set; }
        /// <summary>
        /// Link to university moderator
        /// </summary>
        public UniversityModeratorDTO Moderator { get; set; }
        public bool IsBanned { get; set; }
    }
}
