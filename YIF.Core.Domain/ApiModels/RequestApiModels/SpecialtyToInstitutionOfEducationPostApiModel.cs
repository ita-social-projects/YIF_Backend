namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class SpecialtyToInstitutionOfEducationPostApiModel
    {
        /// <summary>
        /// Specialty Id
        /// </summary>
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string SpecialtyId { get; set; }

        /// <summary>
        /// Institution of education Id
        /// </summary>
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string InstitutionOfEducationId { get; set; }
    }
}
