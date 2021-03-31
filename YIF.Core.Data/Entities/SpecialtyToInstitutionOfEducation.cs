﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class SpecialtyToInstitutionOfEducation
    {
        public string SpecialtyId { get; set; }
        public string InstitutionOfEducationId { get; set; }

        public virtual Specialty Specialty { get; set; }
        public virtual InstitutionOfEducation InstitutionOfEducation { get; set; }
    }
}
