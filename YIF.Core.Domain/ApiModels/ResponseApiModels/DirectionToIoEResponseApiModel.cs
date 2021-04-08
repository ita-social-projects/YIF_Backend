using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class DirectionToIoEResponseApiModel
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
        public IEnumerable<SpecialtyToInstitutionOfEducationResponseApiModel> Specialties { get; set; }
    }
}
