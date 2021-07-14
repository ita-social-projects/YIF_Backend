namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class LectorApiModel
    {
        public string Name { get; set; }

        public string InstitutionOfEducationId { get; set; }
        public string SpecialtyId { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public ImageApiModel ImageApiModel { get; set; }
    }
}
