using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data
{
    public class SpecialtyInUniversityDescription : BaseEntity
    {
        public string EducationalProgramLink { get; set; }

        public string EducationFormId { get; set; }
        public string PaymentFormId { get; set; }

        public string ExamRequirementId { get; set; }

        //public virtual ICollection<ExamRequirement> ExamRequirements { get; set; }

        [ForeignKey("ExamRequirementId")]
        public virtual ExamRequirement ExamRequirement { get; set; }

        [ForeignKey("EducationFormId")]
        public virtual EducationForm EducationForm { get; set; }
        [ForeignKey("PaymentFormId")]
        public virtual PaymentForm PaymentForm { get; set; }


    }
}
