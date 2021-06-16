using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class DepartmentDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<LectorDTO> Lectors { get; set; }
    }
}
