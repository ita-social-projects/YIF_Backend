using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public enum PaymentForm
    {
        Day,
        //Сorrespondence,
        Evening
    }
    public enum EducationForm
    {
        Budget,
        Contract
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
