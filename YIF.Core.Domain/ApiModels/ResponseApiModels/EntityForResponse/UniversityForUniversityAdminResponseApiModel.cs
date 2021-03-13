using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.EntityForResponse
{
    public class InstitutionOfEducationForInstitutionOfEducationAdminResponseApiModel
    {
        /// <summary>
        /// Unique id
        /// </summary>
        /// <example>e2bd4ad9-060b-4d53-8222-9f3e5efbcfc7</example>
        public string Id { get; set; }

        /// <summary>
        /// Name of institutionOfEducation
        /// </summary>
        /// <example>Національний університет водного господарства та природокористування</example>
        public string Name { get; set; }

        /// <summary>
        /// Short name of institutionOfEducation
        /// </summary>
        /// <example>НУВГП</example>
        public string Abbreviation { get; set; }
    }
}
