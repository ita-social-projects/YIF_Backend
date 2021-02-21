using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class DirectionDTO
    {
        /// <summary>
        /// Gets or sets the primary key for this direction.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the direction name for this direction.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the direction code for this direction.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Navigation property for the specialties this direction belongs to.
        /// </summary>
        public virtual IEnumerable<SpecialtyDTO> Specialties { get; set; }
    }
}
