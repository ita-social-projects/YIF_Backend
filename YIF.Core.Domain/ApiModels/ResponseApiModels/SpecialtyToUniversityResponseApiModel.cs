using System.Collections.Generic;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class SpecialtyToUniversityResponseApiModel
    {
        /// <summary>
        /// Gets or sets the specialty name to which this specialty to university belongs.
        /// </summary>
        public string SpecialtyName { get; set; }
        /// <summary>
        /// Gets or sets the specialty code to which this specialty to university belongs.
        /// </summary>
        public string SpecialtyCode { get; set; }
        /// <summary>
        /// Gets or sets the university name to which this specialty to university belongs.
        /// </summary>
        public string UniversityName { get; set; }
        /// <summary>
        /// Gets or sets the education program link to which this specialty to university belongs.
        /// </summary>
        public string EducationalProgramLink { get; set; }
        /// <summary>
        /// Gets or sets the description to which this specialty to university belongs.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Navigation property for the exam requirements this specialty to university belongs to.
        /// </summary>
        public virtual IEnumerable<ExamRequirementsResponseApiModel> ExamRequirements { get; set; }
        /// <summary>
        /// Navigation property for the education form to description this specialty to university belongs to.
        /// </summary>
        public virtual IEnumerable<EducationFormToDescriptionResponseApiModel> EducationFormToDescriptions { get; set; }
        /// <summary>
        /// Navigation property for the payment form to description this specialty to university belongs to.
        /// </summary>
        public virtual IEnumerable<PaymentFormToDescriptionResponseApiModel> PaymentFormToDescriptions { get; set; }
    }
}

