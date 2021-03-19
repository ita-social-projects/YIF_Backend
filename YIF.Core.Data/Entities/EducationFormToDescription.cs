using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class EducationFormToDescription : BaseEntity
    {
        public string EducationFormId { get; set; }
        public string SpecialtyToIoEDescriptionId { get; set; }

        [ForeignKey("EducationFormId")]
        public virtual EducationForm EducationForm { get; set; }
        [ForeignKey("SpecialtyToIoEDescriptionId")]
        public virtual SpecialtyToIoEDescription SpecialtyToIoEDescription { get; set; }
    }
}
