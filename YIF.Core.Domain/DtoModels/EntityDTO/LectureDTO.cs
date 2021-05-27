using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class LectureDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
    }
}
