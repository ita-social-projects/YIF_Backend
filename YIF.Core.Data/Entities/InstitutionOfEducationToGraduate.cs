using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class InstitutionOfEducationToGraduate
    {
        public string GraduateId { get; set; }
        public string InstitutionOfEducationId { get; set; }

        [ForeignKey("GraduateId")]
        public Graduate Graduate { get; set; }
        [ForeignKey("InstitutionOfEducationId")]
        public InstitutionOfEducation InstitutionOfEducation { get; set; }

    }
}
