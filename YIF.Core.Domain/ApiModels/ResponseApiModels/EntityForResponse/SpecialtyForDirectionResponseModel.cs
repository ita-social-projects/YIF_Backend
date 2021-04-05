namespace YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse
{
    public class SpecialtyForDirectionResponseModel
    {
        /// <summary>
        /// Gets or sets the primary key for this specialty.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the specialty name for this specialty.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the direction code for this direction.
        /// </summary>
        public string Code { get; set; }
    }
}
