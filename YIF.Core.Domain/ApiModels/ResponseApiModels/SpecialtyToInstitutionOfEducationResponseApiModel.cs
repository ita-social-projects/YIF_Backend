﻿using System.Collections.Generic;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class SpecialtyToInstitutionOfEducationResponseApiModel
    {
        /// <summary>
        /// Gets or sets the specialty id to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string SpecialtyId { get; set; }

        /// <summary>
        /// Gets or sets the id of specialtyToInstitutionOfEducation entry.
        /// </summary>     
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string SpecialtyToInstitutionOfEducationId { get; set; }

        /// <summary>
        /// Gets or sets the institutionOfEducation id to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string InstitutionOfEducationId { get; set; }

        /// <summary>
        /// Gets or sets the specialty name to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string SpecialtyName { get; set; }

        /// <summary>
        /// Gets or sets the specialty code to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string SpecialtyCode { get; set; }

        /// <summary>
        /// Gets or sets the institutionOfEducation abbreviation to which this specialty to institutionOfEducation belongs.
        /// </summary>
        public string InstitutionOfEducationAbbreviation { get; set; }

        /// <summary>
        /// Field to determine if there is a specialty and institutionOfEducation in favorites
        /// </summary>
        public bool IsFavorite { get; set; }

        /// <summary>
        /// Field to determine if specialty is deleted in this institutionOfEducation
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Navigation property for the specialty descriptions this specialty to institutionOfEducation belongs to.
        /// </summary>
        public IEnumerable<SpecialtyToIoEDescriptionResponseApiModel> Descriptions { get; set; }
    }
}

