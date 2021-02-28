using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class EducationFormToDescription : BaseEntity
    {
        public string Description { get; set; }
        public string EducationFormId { get; set; }
        public string SpecialtyInUniversityDescriptionId { get; set; }

        [ForeignKey("EducationFormId")]
        public virtual EducationForm EducationForm { get; set; }
        [ForeignKey("SpecialtyInUniversityDescriptionId")]
        public virtual SpecialtyInUniversityDescription SpecialtyInUniversityDescription { get; set; }
    }
}
