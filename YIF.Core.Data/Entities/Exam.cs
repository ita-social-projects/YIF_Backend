using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public class Exam : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<ExamRequirement> ExamRequirements { get; set; }
    }
}
