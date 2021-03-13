using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class InstitutionOfEducationToGraduate
    {
        public string GraduateId { get; set; }
        public string InstitutionOfEducationId { get; set; }

        [ForeignKey("GraduateId")]
        public virtual Graduate Graduate { get; set; }
        [ForeignKey("InstitutionOfEducationId")]
        public virtual InstitutionOfEducation InstitutionOfEducation { get; set; }

    }
}
