using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyInUniversityDescriptionDTO
    {
        public string Id { get; set; }
        public string EducationalProgramLink { get; set; }

        public virtual ICollection<SpecialtyToUniversityDTO> SpecialtyToUniversities { get; set; }

        public virtual ICollection<ExamRequirementDTO> ExamRequirements { get; set; }

        public virtual ICollection<PaymentFormToDescriptionDTO> PaymentFormToDescriptions { get; set; }

        public virtual ICollection<EducationFormToDescriptionDTO> EducationFormToDescriptions { get; set; }
    }
}