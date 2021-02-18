using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YIF.Core.Data.Entities
{
    public class ExamRequirement: BaseEntity
    {
        public string ExamId { get; set; }

        public string SpecialtyInUniversityDescriptionId { get; set; }
        
        public double MinimumScore { get; set; }

        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }

        [ForeignKey("SpecialtyInUniversityDescriptionId")]
        public virtual SpecialtyInUniversityDescription SpecialtyInUniversityDescription { get; set; }

    }
}
