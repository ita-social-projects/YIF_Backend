namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class ExamRequirementsResponseApiModel
    {
        /// <summary>
        /// Gets or sets the exam name to which this exam requirement belongs.
        /// </summary>
        public string ExamName { get; set; }
        /// <summary>
        /// Gets or sets the minimum score for this exam requirement.
        /// </summary>
        public double MinimumScore { get; set; }
        /// <summary>
        /// Gets or sets the coefficient for this exam requirement.
        /// </summary>
        public double Coefficient { get; set; }
    }
}
