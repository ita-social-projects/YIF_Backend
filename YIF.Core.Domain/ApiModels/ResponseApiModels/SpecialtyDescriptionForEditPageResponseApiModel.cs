using System.Collections.Generic;
using YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class SpecialtyDescriptionForEditPageResponseApiModel
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
        /// Gets or sets the specialty description id to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string SpecialtyToIoEDescriptionId { get; set; }
        /// <summary>
        /// Gets or sets the education program link to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string EducationalProgramLink { get; set; }

        /// <summary>
        /// Gets or sets the description to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Navigation property for the exam requirements this specialty to institutionOfEducation belongs to.
        /// </summary>
        public virtual IEnumerable<ExamRequirementForEditPageResponseApiModel> ExamRequirements { get; set; }
        /// <summary>
        /// Navigation property for the education forms this specialty to institutionOfEducation belongs to.
        /// </summary>
        public virtual IEnumerable<EducationFormToDescriptionForEditPageResponseApiModel> EducationFormsToDescription { get; set; }
        /// <summary>
        /// Navigation property for the payment forms this specialty to institutionOfEducation belongs to.
        /// </summary>
        public virtual IEnumerable<PaymentFormToDescriptionForEditPageResponseApiModel> PaymentFormsToDescription { get; set; }
    }
}
