using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class SpecialtyToGraduate : BaseEntity
    {
        public string SpecialtyId { get; set; }
        public string GraduateId { get; set; }

        [ForeignKey("SpecialtyId")]
        public virtual Specialty Specialty { get; set; }
        [ForeignKey("GraduateId")]
        public virtual Graduate Graduate { get; set; }
    }
}
