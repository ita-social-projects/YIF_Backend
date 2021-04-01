﻿using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyToInstitutionOfEducationDTO
    {
        public string Id { get; set; }
        public string SpecialtyId { get; set; }
        public string InstitutionOfEducationId { get; set; }

        public virtual SpecialtyDTO Specialty { get; set; }
        public virtual InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
        public virtual ICollection<SpecialtyToIoEDescriptionDTO> SpecialtyToIoEDescriptions { get; set; }

    }
}
