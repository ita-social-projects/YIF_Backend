using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialityDTO
    {
        /// <summary>
        /// Speciality name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Id of the speciality direction
        /// </summary>
        public string DirectionId { get; set; }
        /// <summary>
        /// Speciality description
        /// </summary>
        public string Description { get; set; }

        public virtual DirectionDTO Direction { get; set; }
    }
}
