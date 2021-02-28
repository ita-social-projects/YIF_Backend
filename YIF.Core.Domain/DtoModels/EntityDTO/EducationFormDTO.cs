using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class EducationFormDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EducationFormToDescriptionDTO> EducationFormToDescriptions { get; set; }
    }
}