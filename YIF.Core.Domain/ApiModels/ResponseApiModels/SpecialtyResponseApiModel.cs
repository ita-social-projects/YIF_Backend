namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class SpecialtyResponseApiModel
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

        /// <summary>
        /// Gets or sets the description for this specialty.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the direction id to which this specialty belongs.
        /// </summary>
        public string DirectionId { get; set; }

        /// <summary>
        /// Gets or sets the direction name to which this specialty belongs.
        /// </summary>
        public string DirectionName { get; set; }

        /// <summary>
        /// Field to determine if there is a specialty in favorites
        /// </summary>
        public bool IsFavorite { get; set; }
    }
}
