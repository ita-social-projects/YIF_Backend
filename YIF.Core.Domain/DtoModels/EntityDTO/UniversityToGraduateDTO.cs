using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class UniversityToGraduateDTO
    {
        public string GraduateId { get; set; }
        public string UniversityId { get; set; }

        public virtual GraduateDTO Graduate { get; set; }
        public virtual UniversityDTO University { get; set; }
    }
}
