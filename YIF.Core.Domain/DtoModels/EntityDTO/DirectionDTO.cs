using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class DirectionDTO
    {
        public string Id { get; set; }
        /// <summary>
        /// Speciality name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// List of specialities
        /// </summary>
        public virtual IEnumerable<SpecialityDTO> Specialities { get; set; }
    }
}
