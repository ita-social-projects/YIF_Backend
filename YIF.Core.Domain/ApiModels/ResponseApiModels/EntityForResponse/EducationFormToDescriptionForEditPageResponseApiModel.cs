namespace YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse
{
    public class EducationFormToDescriptionForEditPageResponseApiModel
    {
        /// <summary>
        /// Get the education form id to which this education form to description belongs.
        /// </summary>
        public string EducationFormId { get; set; }
        /// <summary>
        /// Get the education form name for this education form to description form.
        /// </summary>
        public string EducationFormName { get; set; }
    }
}
