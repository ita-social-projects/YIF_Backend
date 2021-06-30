using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class DisciplineDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LectorId { get; set; }
        public LectorDTO Lector { get; set; }
        public string SpecialityId { get; set; }
        public SpecialtyDTO Speciality { get; set; }

    }
}
