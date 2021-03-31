using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class SpecialtyToInstitutionOfEducation : BaseEntity
    {
        public string SpecialtyId { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public string SpecialtyToIoEDescriptionId { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("SpecialtyToIoEDescriptionId")]
        public virtual SpecialtyToIoEDescription SpecialtyToIoEDescription { get; set; }
        [ForeignKey("SpecialtyId")]
        public virtual Specialty Specialty { get; set; }
        [ForeignKey("InstitutionOfEducationId")]
        public virtual InstitutionOfEducation InstitutionOfEducation { get; set; }
    }
}
