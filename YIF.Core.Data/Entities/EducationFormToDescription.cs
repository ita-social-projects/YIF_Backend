﻿using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class EducationFormToDescription : BaseEntity
    {
        public string EducationFormId { get; set; }
        public string SpecialtyInInstitutionOfEducationDescriptionId { get; set; }

        [ForeignKey("EducationFormId")]
        public virtual EducationForm EducationForm { get; set; }
        [ForeignKey("SpecialtyInInstitutionOfEducationDescriptionId")]
        public virtual SpecialtyInInstitutionOfEducationDescription SpecialtyInInstitutionOfEducationDescription { get; set; }
    }
}
