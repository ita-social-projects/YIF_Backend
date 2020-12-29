using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class DirectionDTO
    {
        /// <summary>
        /// Speciality name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// List of specialities
        /// </summary>
        public virtual IQueryable<SpecialityDTO> Specialities { get; set; }
    }
}
