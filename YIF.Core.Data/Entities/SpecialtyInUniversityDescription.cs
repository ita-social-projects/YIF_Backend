﻿using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public class SpecialtyInUniversityDescription : BaseEntity
    {
        public string EducationalProgramLink { get; set; }
        public string Description { get; set; }

        public virtual ICollection<SpecialtyToUniversity> SpecialtyToUniversities { get; set; }
        public virtual ICollection<ExamRequirement> ExamRequirements { get; set; }
        public virtual ICollection<PaymentFormToDescription> PaymentFormToDescriptions { get; set; }
        public virtual ICollection<EducationFormToDescription> EducationFormToDescriptions { get; set; }
    }
}
