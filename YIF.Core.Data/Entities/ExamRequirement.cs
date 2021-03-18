namespace YIF.Core.Data.Entities
{
    public class ExamRequirement : BaseEntity
    {
        public string ExamId { get; set; }
        public string SpecialtyInInstitutionOfEducationDescriptionId { get; set; }
        public double MinimumScore { get; set; }
        public double Coefficient { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual SpecialtyInInstitutionOfEducationDescription SpecialtyInInstitutionOfEducationDescription { get; set; }
    }
}
