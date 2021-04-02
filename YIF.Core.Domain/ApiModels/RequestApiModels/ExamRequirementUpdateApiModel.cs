namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class ExamRequirementUpdateApiModel
    {
        public string ExamId { get; set; }
        public string SpecialtyToIoEDescriptionId { get; set; }
        public double MinimumScore { get; set; }
        public double Coefficient { get; set; }
    }
}
