using System.Collections.Generic;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyToIoEDescriptionDTO
    {
        public string Id { get; set; }
        public string SpecialtyToInstitutionOfEducationId { get; set; }
        public PaymentForm PaymentForm { get; set; }
        public EducationForm EducationForm { get; set; }
        public string EducationalProgramLink { get; set; }
        public string Description { get; set; }

        public SpecialtyToInstitutionOfEducation SpecialtyToInstitutionOfEducation { get; set; }
        public ICollection<ExamRequirementDTO> ExamRequirements { get; set; }
    }
}