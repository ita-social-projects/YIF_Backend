using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class Specialty : BaseEntity
    {
        public string Name { get; set; }
        public string DirectionId { get; set; }
        public string Description { get; set; }

        public string Code { get; set; }

        [ForeignKey("DirectionId")]
        public virtual Direction Direction { get; set; }
    }
}
