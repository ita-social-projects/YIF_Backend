using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class ExamDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ExamRequirementDTO> ExamRequirements { get; set; }
    }
}