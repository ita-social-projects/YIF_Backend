using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class LectorDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool IsDeleted { get; set; } = false;
        public UserDTO User { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
        public string DepartmentId { get; set; }
        public DepartmentDTO Department { get; set; }
        public ImageApiModel ImageApiModel { get; set; }
    }
}
