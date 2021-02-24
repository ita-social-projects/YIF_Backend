using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class EducationFormToDescription : BaseEntity
    {
        public string EducationFormId { get; set; }
        public string SpecialtyInUniversityDescriptionId { get; set; }

        [ForeignKey("EducationFormId")]
        public EducationForm EducationForm { get; set; }
        [ForeignKey("SpecialtyInUniversityDescriptionId")]
        public SpecialtyInUniversityDescription SpecialtyInUniversityDescription { get; set; }
    }
}
