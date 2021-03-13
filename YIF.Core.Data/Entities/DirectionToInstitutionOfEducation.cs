using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class DirectionToInstitutionOfEducation : BaseEntity
    {
        public string DirectionId { get; set; }
        public string InstitutionOfEducationId { get; set; }

        [ForeignKey("DirectionId")]
        public virtual Direction Direction { get; set; }
        [ForeignKey("InstitutionOfEducationId")]
        public virtual InstitutionOfEducation InstitutionOfEducation { get; set; }
    }
}
