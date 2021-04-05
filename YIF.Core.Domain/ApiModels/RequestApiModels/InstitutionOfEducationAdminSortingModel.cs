namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class InstitutionOfEducationAdminSortingModel
    {
        /// <summary>
        /// Users name
        /// </summary>
        public bool? UserName { get; set; } = null;

        /// <summary>
        /// Email
        /// </summary>
        public bool? Email { get; set; } = null;

        /// <summary>
        /// Institution of education name
        /// </summary>
        public bool? InstitutionOfEducationName { get; set; } = null;

        /// <summary>
        /// Status of banning of administrator
        /// </summary>
        public bool? IsBanned { get; set; } = null;
    }
}
