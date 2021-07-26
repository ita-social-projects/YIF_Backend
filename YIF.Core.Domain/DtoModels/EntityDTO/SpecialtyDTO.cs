using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyDTO
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
        /// Gets or sets the specialty code for this specialty.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the direction id to which this specialty belongs
        /// </summary>
        public string DirectionId { get; set; }
        /// <summary>
        /// Whether Specialty is deleted or not
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Gets or sets the description for this specialty
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Navigation property for the direction to which this specialty belongs.
        /// </summary>
        public DirectionDTO Direction { get; set; }
        /// <summary>
        /// Navigation property for the disciplines
        /// </summary>
        public ICollection<DisciplineDTO> Disciplines { get; set; }
    }
}
