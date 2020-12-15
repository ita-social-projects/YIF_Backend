using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YIF.Core.Data.Entities
{
    public class DirectionToUniversity : BaseEntity
    {
        public string DirectionId { get; set; }
        public string UniversityId { get; set; }

        [ForeignKey("DirectionId")]
        public virtual Direction Speciality { get; set; }
        [ForeignKey("UniversityId")]
        public virtual University University { get; set; }
    }
}
