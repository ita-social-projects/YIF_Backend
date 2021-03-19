using System.Collections.Generic;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class SpecialtyToIoEDescriptionResponseApiModel
    {
        /// <summary>
        /// Gets or sets the educational program link for this description.
        /// </summary>
        public string EducationalProgramLink { get; set; }
        /// <summary>
        /// Gets or sets description for this description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Navigation property for the exam requirement this description belongs to.
        /// </summary>
        public virtual IEnumerable<ExamRequirementsResponseApiModel> ExamRequirements { get; set; }
        /// <summary>
        /// Navigation property for the payment form to description this description belongs to.
        /// </summary>
        public virtual IEnumerable<PaymentFormToDescriptionResponseApiModel> PaymentFormToDescriptions { get; set; }
        /// <summary>
        /// Navigation property for the education form to description this description belongs to.
        /// </summary>
        public virtual IEnumerable<EducationFormToDescriptionResponseApiModel> EducationFormToDescriptions { get; set; }
    }
}