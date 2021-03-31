using System.Collections.Generic;
using System.Text.Json.Serialization;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class SpecialtyToIoEDescriptionResponseApiModel
    {
        /// <summary>
        /// Type of education form
        /// </summary>  
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EducationForm EducationForm { get; set; }

        /// <summary>
        /// Type of payment form
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentForm PaymentForm { get; set; }

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
    }
}