using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialityToUniversityDTO
    {
        /// <summary>
        /// Id of direction
        /// </summary>
        public string DirectionId { get; set; }
        /// <summary>
        /// Id of university
        /// </summary>
        public string UniversityId { get; set; }

        public virtual DirectionDTO Direction { get; set; }
        public virtual UniversityDTO University { get; set; }
    }
}
