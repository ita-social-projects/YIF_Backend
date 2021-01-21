using System.Collections.Generic;

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
    }
}
