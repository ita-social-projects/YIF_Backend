namespace YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse
{
    public class ExamRequirementForEditPageResponseApiModel
    {
        /// <summary>
        /// Get the exam id to which this exam requirement belongs.
        /// </summary>
        public string ExamId { get; set; }
        /// <summary>
        /// Get the exam name to which this exam requirement belongs.
        /// </summary>
        public string ExamName { get; set; }
        /// <summary>
        /// Get the minimum score for this exam requirement.
        /// </summary>
        public double MinimumScore { get; set; }
        /// <summary>
        /// Get the coefficient for this exam requirement.
        /// </summary>
        public double Coefficient { get; set; }
    }
}
