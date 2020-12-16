using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Data.Entities
{
    public class Direction : BaseEntity
    {
        public string Name { get; set; }

        public virtual List<Speciality> Specialities { get; set; }
    }
}
