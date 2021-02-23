using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public class Direction : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public virtual List<Specialty> Specialties { get; set; }
    }
}
