using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public class Exam : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<ExamRequirement> ExamRequirements { get; set; }
    }
}
