using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class SpecialtyToUniversity : BaseEntity
    {
        public string SpecialtyId { get; set; }
        public string UniversityId { get; set; }

        [ForeignKey("SpecialtyId")]
        public virtual Specialty Specialty { get; set; }
        [ForeignKey("UniversityId")]
        public virtual University University { get; set; }
    }
}
