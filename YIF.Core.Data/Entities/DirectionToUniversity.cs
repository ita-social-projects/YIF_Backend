using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class DirectionToUniversity : BaseEntity
    {
        public string DirectionId { get; set; }
        public string UniversityId { get; set; }

        [ForeignKey("DirectionId")]
        public virtual Direction Direction { get; set; }
        [ForeignKey("UniversityId")]
        public virtual University University { get; set; }
    }
}
