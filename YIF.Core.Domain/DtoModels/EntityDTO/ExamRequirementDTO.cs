namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class ExamRequirementDTO
    {
        public string Id { get; set; }
        public string ExamId { get; set; }
        public string SpecialtyInInstitutionOfEducationDescriptionId { get; set; }
        public double MinimumScore { get; set; }
        public double Coefficient { get; set; }

        public virtual ExamDTO Exam { get; set; }
        public virtual SpecialtyInInstitutionOfEducationDescriptionDTO SpecialtyInInstitutionOfEducationDescription { get; set; }
    }
}