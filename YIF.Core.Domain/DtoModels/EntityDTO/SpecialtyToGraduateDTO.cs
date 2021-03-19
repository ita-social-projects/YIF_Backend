using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyToGraduateDTO
    {
        public string SpecialtyId { get; set; }
        public string GraduateId { get; set; }

        public virtual SpecialtyDTO Specialty { get; set; }
        public virtual GraduateDTO Graduate { get; set; }
    }
}
