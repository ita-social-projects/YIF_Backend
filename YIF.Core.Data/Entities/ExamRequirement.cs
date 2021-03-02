namespace YIF.Core.Data.Entities
{
    public class ExamRequirement : BaseEntity
    {
        public string ExamId { get; set; }
        public string SpecialtyInUniversityDescriptionId { get; set; }
        public double MinimumScore { get; set; }
        public double Coefficient { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual SpecialtyInUniversityDescription SpecialtyInUniversityDescription { get; set; }
    }
}
