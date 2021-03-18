using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class LectureDTO
    {
        public string Id { get; set; }
        public string InstitutionOfEducationId { get; set; }

        public InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
        /// <summary>
        /// Link to Identity user
        /// </summary>
        public UserDTO User { get; set; }
    }
}
