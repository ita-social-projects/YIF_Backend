using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public class SpecialtyInUniversityDescription : BaseEntity
    {
        public string EducationalProgramLink { get; set; }

        public ICollection<ExamRequirement> ExamRequirements { get; set; }

        public ICollection<PaymentFormToDescription> PaymentFormToDescriptions { get; set; }

        public ICollection<EducationFormToDescription> EducationFormToDescriptions { get; set; }
    }
}
