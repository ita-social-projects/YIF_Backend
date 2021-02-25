using System.Collections.Generic;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyInUniversityDescriptionDTO
    {
        public string Id { get; set; }
        public string EducationalProgramLink { get; set; }

        public ICollection<ExamRequirementDTO> ExamRequirements { get; set; }

        public ICollection<PaymentFormToDescriptionDTO> PaymentFormToDescriptions { get; set; }

        public ICollection<EducationFormToDescriptionDTO> EducationFormToDescriptions { get; set; }
    }
}