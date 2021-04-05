using System.Collections.Generic;
using System.Text.Json.Serialization;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class SpecialtyDescriptionUpdateApiModel
    {
        /// <summary>
        /// Id
        /// </summary>     
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string Id { get; set; }

        /// <summary>
        /// SpecialtyToInstitutionOfEducation Id
        /// </summary>     
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string SpecialtyToInstitutionOfEducationId { get; set; }

        /// <summary>
        /// Payment form for specialty
        /// </summary>     
        /// <example>Budget</example>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentForm PaymentForm { get; set; }

        /// <summary>
        /// Education form for specialty
        /// </summary>     
        /// <example>Day</example>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EducationForm EducationForm { get; set; }

        /// <summary>
        /// Link for specialty
        /// </summary>     
        /// <example>example@gmail.com</example>
        public string EducationalProgramLink { get; set; }

        /// <summary>
        /// Description for specialty
        /// </summary>     
        /// <example>This is custom description.</example>
        public string Description { get; set; }

        /// <summary>
        /// List of exams for specialty
        /// </summary>     
        public ICollection<ExamRequirementUpdateApiModel> ExamRequirements { get; set; }
    }
}
