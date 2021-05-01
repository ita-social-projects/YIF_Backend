namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class FilterApiModel
    {
        /// <summary>
        /// Direction name
        /// </summary>
        /// <example>Computer Sciences</example>
        public string DirectionName { get; set; }

        /// <summary>
        /// Institution of education name
        /// </summary>
        /// <example>Kyiv Polytechnic Institute</example>
        public string InstitutionOfEducationName { get; set; }

        /// <summary>
        /// Specialty name
        /// </summary>
        /// <example>Economics</example>
        public string SpecialtyName { get; set; }

        /// <summary>
        /// Institution of education abbreviation
        /// </summary>
        /// <example>KPI</example>
        public string InstitutionOfEducationAbbreviation { get; set; }

        /// <summary>
        /// Payment Form
        /// </summary>
        /// <example>Governmental</example>
        public string PaymentForm { get; set; }

        /// <summary>
        /// Education form
        /// </summary>
        /// <example>Daily</example>
        public string EducationForm { get; set; }

        /// <summary>
        /// Institution of education type
        /// </summary>
        /// <example>University</example>
        public string InstitutionOfEducationType { get; set; }
    }
}
