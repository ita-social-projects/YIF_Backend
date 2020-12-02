using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Data.Entities
{
    public class Speciality : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// List of universities, who have`s this speciality
        /// </summary>
        //public ICollection<SpecialityToUniversity> Universities { get; set; }
    }
}
