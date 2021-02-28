namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class ExamRequirementDTO
    {
        public string Id { get; set; }
        public string ExamId { get; set; }

        public string SpecialtyInUniversityDescriptionId { get; set; }

        public double MinimumScore { get; set; }
        public double Coefficient { get; set; }

        public ExamDTO Exam { get; set; }

        public SpecialtyInUniversityDescriptionDTO SpecialtyInUniversityDescription { get; set; }
    }
}