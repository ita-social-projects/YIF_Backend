using System.Collections.Generic;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class SpecialtyNamesResponseApiModel
    {
        /// <summary>
        /// A class for return the list of specialties names in response.
        /// </summary>
        /// <param name="specialitiesNames">The list of specialties names</param>
        public SpecialtyNamesResponseApiModel(List<string> specialitiesNames = null)
        {
            SpecialitiesNames = specialitiesNames;
        }
        /// <summary>
        /// Gets or sets the specialties names.
        /// </summary>
        public List<string> SpecialitiesNames { get; set; }
    }
}
