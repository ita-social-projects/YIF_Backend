using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public class SpecialtyToInstitutionOfEducation: BaseEntity 
    {
        public string SpecialtyId { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Specialty Specialty { get; set; }
        public InstitutionOfEducation InstitutionOfEducation { get; set; }
        public ICollection<SpecialtyToIoEDescription> SpecialtyToIoEDescriptions { get; set; }
    }
}
