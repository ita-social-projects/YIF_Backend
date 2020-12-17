using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class SpecialityToUniversity : BaseEntity
    {
        public string SpecialityId { get; set; }
        public string UniversityId { get; set; }

        [ForeignKey("SpecialityId")]
        public virtual Speciality Speciality { get; set; }
        [ForeignKey("UniversityId")]
        public virtual University University { get; set; }
    }
}
