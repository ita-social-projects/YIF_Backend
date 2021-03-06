namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class SpecialtyForDirectionResponseModel
    {
        /// <summary>
        /// Gets or sets the primary key for this specialty.
        /// </summary>
        public string SpecialtyId { get; set; }
        /// <summary>
        /// Gets or sets the specialty name for this specialty.
        /// </summary>
        public string SpecialtyName { get; set; }
        /// <summary>
        /// Get or set the direction code for this direction.
        /// </summary>
        public string SpecialtyCode { get; set; }
    }
}
