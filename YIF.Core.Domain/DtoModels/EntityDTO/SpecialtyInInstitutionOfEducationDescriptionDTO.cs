using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyToIoEDescriptionDTO
    {
        public string Id { get; set; }
        public string EducationalProgramLink { get; set; }
        public string Description { get; set; }

        public virtual ICollection<SpecialtyToInstitutionOfEducationDTO> SpecialtyToInstitutionOfEducations { get; set; }

        public virtual ICollection<ExamRequirementDTO> ExamRequirements { get; set; }

        public virtual ICollection<PaymentFormToDescriptionDTO> PaymentFormToDescriptions { get; set; }

        public virtual ICollection<EducationFormToDescriptionDTO> EducationFormToDescriptions { get; set; }
    }
}