namespace YIF.Core.Data.Entities
{
    public class ExamRequirement : BaseEntity
    {
        public string ExamId { get; set; }

        public string SpecialtyInUniversityDescriptionId { get; set; }

        public double MinimumScore { get; set; }

        public Exam Exam { get; set; }

        public SpecialtyInUniversityDescription SpecialtyInUniversityDescription { get; set; }
    }
}
