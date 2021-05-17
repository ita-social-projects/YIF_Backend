using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public enum PaymentForm
    {
        Governmental,
        Contract 
    }
    public enum EducationForm
    {
        Daily,
        Remote
    }
    public class SpecialtyToIoEDescription : BaseEntity
    {
        public string SpecialtyToInstitutionOfEducationId { get; set; }
        public PaymentForm PaymentForm { get; set; }
        public EducationForm EducationForm { get; set; }
        public string EducationalProgramLink { get; set; }
        public string Description { get; set; }

        public SpecialtyToInstitutionOfEducation SpecialtyToInstitutionOfEducation { get; set; }
        public ICollection<ExamRequirement> ExamRequirements { get; set; }
    }
}
