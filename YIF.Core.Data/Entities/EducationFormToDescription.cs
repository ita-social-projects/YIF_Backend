using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YIF.Core.Data.Entities
{
    public class EducationFormToDescription:BaseEntity
    {
        public string EducationFormId { get; set; }

        public string SpecialtyInUniversityDescriptionId { get; set; }
        
        public EducationForm EducationForm { get; set; }
        
        public SpecialtyInUniversityDescription SpecialtyInUniversityDescription { get; set; }
    }
}
