using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class SpecialtyToInstitutionOfEducation: BaseEntity 
    {
        public string SpecialtyId { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Specialty Specialty { get; set; }
        public virtual InstitutionOfEducation InstitutionOfEducation { get; set; }
        public virtual ICollection<SpecialtyToIoEDescription> SpecialtyToIoEDescriptions { get; set; }
    }
}
