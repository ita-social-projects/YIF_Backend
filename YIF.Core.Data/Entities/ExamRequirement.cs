namespace YIF.Core.Data.Entities
{
    public class ExamRequirement : BaseEntity
    {
        public string ExamId { get; set; }
        public string SpecialtyToIoEDescriptionId { get; set; }
        public double MinimumScore { get; set; }
        public double Coefficient { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual SpecialtyToIoEDescription SpecialtyToIoEDescription { get; set; }
    }
}
