namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class ExamRequirementUpdateApiModel
    {
        /// <summary>
        /// Exam Id
        /// </summary>     
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string ExamId { get; set; }

        /// <summary>
        /// SpecialtyToIoEDescription Id
        /// </summary>     
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string SpecialtyToIoEDescriptionId { get; set; }

        /// <summary>
        /// Minimum score for exam
        /// </summary>     
        /// <example>150</example>
        public double MinimumScore { get; set; }

        /// <summary>
        /// Coefficient for exam
        /// </summary>     
        /// <example>0.3</example>
        public double Coefficient { get; set; }
    }
}
