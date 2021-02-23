using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class SpecialtyInUniversityDescription : BaseEntity
    {
        public string EducationalProgramLink { get; set; }

        public ICollection<ExamRequirement> ExamRequirements { get; set; }

        public ICollection<PaymentFormToDescription> PaymentFormToDescriptions { get; set; }

        public  ICollection<EducationFormToDescription> EducationFormToDescriptions { get; set; }
        
    }
}
