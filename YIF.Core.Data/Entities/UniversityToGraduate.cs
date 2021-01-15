using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class UniversityToGraduate
    {
        public string GraduateId { get; set; }
        public string UniversityId { get; set; }

        [ForeignKey("GraduateId")]
        public virtual Graduate Graduate { get; set; }
        [ForeignKey("UniversityId")]
        public virtual University University { get; set; }

    }
}
