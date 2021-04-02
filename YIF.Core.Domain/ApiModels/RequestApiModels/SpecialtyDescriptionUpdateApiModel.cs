using System.Collections.Generic;
using System.Text.Json.Serialization;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class SpecialtyDescriptionUpdateApiModel
    {
        public string Id { get; set; }
        public string SpecialtyToInstitutionOfEducationId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentForm PaymentForm { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EducationForm EducationForm { get; set; }
        public string EducationalProgramLink { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ExamRequirementUpdateApiModel> ExamRequirements { get; set; }
    }
}
