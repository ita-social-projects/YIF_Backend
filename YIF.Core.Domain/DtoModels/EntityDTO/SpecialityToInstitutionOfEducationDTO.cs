using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialityToInstitutionOfEducationDTO
    {
        public string Id { get; set; }
        public string SpecialityId { get; set; }
        public string InstitutionOfEducationId { get; set; }

        public SpecialtyDTO Speciality { get; set; }
        public InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
    }
}
