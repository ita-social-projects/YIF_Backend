namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class ExamRequirementDTO
    {
        public string Id { get; set; }
        public string ExamId { get; set; }
        public string SpecialtyToIoEDescriptionId { get; set; }
        public double MinimumScore { get; set; }
        public double Coefficient { get; set; }

        public virtual ExamDTO Exam { get; set; }
        public virtual SpecialtyToIoEDescriptionDTO SpecialtyToIoEDescription { get; set; }
    }
}