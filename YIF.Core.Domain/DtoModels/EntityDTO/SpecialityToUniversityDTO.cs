using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialityToUniversityDTO
    {
        public string Id { get; set; }
        public string SpecialityId { get; set; }
        public string UniversityId { get; set; }

        public SpecialtyDTO Speciality { get; set; }
        public UniversityDTO University { get; set; }
    }
}
