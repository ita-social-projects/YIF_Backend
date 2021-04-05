using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyToInstitutionOfEducationDTO
    {
        public string Id { get; set; }
        public string SpecialtyId { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public bool IsDeleted { get; set; }

        public SpecialtyDTO Specialty { get; set; }
        public InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
        public ICollection<SpecialtyToIoEDescriptionDTO> SpecialtyToIoEDescriptions { get; set; }
    }
}
