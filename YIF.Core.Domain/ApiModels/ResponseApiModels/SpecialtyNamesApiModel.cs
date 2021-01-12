using System.Collections.Generic;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class SpecialtyNamesApiModel
    {
        /// <summary>
        /// Gets or sets the specialties names.
        /// </summary>
        IEnumerable<string> SpecialitiesNames { get; set; }
    }
}
