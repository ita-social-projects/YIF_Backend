using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Lector> Lectors { get; set; }
    }
}
