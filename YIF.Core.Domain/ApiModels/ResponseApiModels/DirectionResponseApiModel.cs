using System.Collections.Generic;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class DirectionResponseApiModel
    {
        /// <summary>
        /// Get or set the primary key for this direction.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Get or set the direction name for this direction.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Get or set the direction code for this direction.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Navigation property for the specialties this direction belongs to.
        /// </summary>
        public virtual IEnumerable<SpecialtyForDirectionResponseModel> Specialties { get; set; }
    }
}
