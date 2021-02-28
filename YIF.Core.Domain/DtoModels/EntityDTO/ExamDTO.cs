using System.Collections.Generic;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class ExamDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<ExamRequirement> ExamRequirements { get; set; }
    }
}