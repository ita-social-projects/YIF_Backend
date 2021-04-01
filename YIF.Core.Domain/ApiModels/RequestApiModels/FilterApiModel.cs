using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class FilterApiModel
    {
        public string DirectionName { get; set; }
        public string InstitutionOfEducationName { get; set; }
        public string SpecialtyName { get; set; }
        public string InstitutionOfEducationAbbreviation { get; set; }
        public string PaymentForm { get; set; }
        public string EducationForm { get; set; }
    }
}
