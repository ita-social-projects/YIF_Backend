using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public class EducationForm : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<EducationFormToDescription> EducationFormToDescriptions { get; set; }
    }
}
