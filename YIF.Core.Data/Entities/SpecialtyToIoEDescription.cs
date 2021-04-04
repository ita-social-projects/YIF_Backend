using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public enum PaymentForm
    {
        Budget,
        Contract
    }
    public enum EducationForm
    {
        Day,
        Evening
    }
    public class SpecialtyToIoEDescription : BaseEntity
    {
        public string SpecialtyToInstitutionOfEducationId { get; set; }
        public PaymentForm PaymentForm { get; set; }
        public EducationForm EducationForm { get; set; }
        public string EducationalProgramLink { get; set; }
        public string Description { get; set; }

        public virtual SpecialtyToInstitutionOfEducation SpecialtyToInstitutionOfEducation { get; set; }
        public virtual ICollection<ExamRequirement> ExamRequirements { get; set; }

    }
}
