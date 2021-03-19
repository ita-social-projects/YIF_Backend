using System.Collections.Generic;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class SpecialtyToInstitutionOfEducationResponseApiModel
    {
        /// <summary>
        /// Gets or sets the specialty id to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string SpecialtyId { get; set; }
        /// <summary>
        /// Gets or sets the institutionOfEducation id to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string InstitutionOfEducationId { get; set; }
        /// <summary>
        /// Gets or sets the specialty name to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string SpecialtyName { get; set; }
        /// <summary>
        /// Gets or sets the specialty code to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string SpecialtyCode { get; set; }
        /// <summary>
        /// Gets or sets the institutionOfEducation abbreviation to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string InstitutionOfEducationAbbreviation { get; set; }
        /// <summary>
        /// Gets or sets the education program link to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string EducationalProgramLink { get; set; }
        /// <summary>
        /// Field to determine if there is a specialty and institutionOfEducation in favorites
        /// </summary>
        public bool IsFavorite { get; set; }
        /// <summary>
        /// Gets or sets the description to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Navigation property for the exam requirements this specialty to institutionOfEducation belongs to.
        /// </summary>
        public virtual IEnumerable<ExamRequirementsResponseApiModel> ExamRequirements { get; set; }
        /// <summary>
        /// Navigation property for the education form to description this specialty to institutionOfEducation belongs to.
        /// </summary>
        public virtual IEnumerable<EducationFormToDescriptionResponseApiModel> EducationFormToDescriptions { get; set; }
        /// <summary>
        /// Navigation property for the payment form to description this specialty to institutionOfEducation belongs to.
        /// </summary>
        public virtual IEnumerable<PaymentFormToDescriptionResponseApiModel> PaymentFormToDescriptions { get; set; }
    }
}

