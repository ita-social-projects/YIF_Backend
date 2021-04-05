using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public class Specialty : BaseEntity
    {
        public string Name { get; set; }
        public string DirectionId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        [ForeignKey("DirectionId")]
        public Direction Direction { get; set; }
        public ICollection<SpecialtyToInstitutionOfEducationToGraduate> SpecialtyToInstitutionOfEducationToGraduates { get; set; }
        public ICollection<SpecialtyToGraduate> SpecialtyToGraduates { get; set; }
        public ICollection<SpecialtyToInstitutionOfEducation> SpecialtyToInstitutionOfEducations { get; set; }
    }
}
