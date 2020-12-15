using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YIF.Core.Data.Entities
{
    public class Speciality : BaseEntity
    {
        public string Name { get; set; }
        public string DirectionId { get; set; }
        public string Description { get; set; }

        [ForeignKey("DirectionId")]
        public virtual Direction Direction { get; set; }
        /// <summary>
        /// List of universities, who have`s this speciality
        /// </summary>
        //public ICollection<SpecialityToUniversity> Universities { get; set; }
    }
}
