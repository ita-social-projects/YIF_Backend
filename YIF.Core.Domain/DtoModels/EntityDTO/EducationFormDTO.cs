using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class EducationFormDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<EducationFormToDescriptionDTO> EducationFormToDescriptions { get; set; }
    }
}