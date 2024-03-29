﻿using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class SpecialtyToGraduate
    {
        public string SpecialtyId { get; set; }
        public string GraduateId { get; set; }

        [ForeignKey("SpecialtyId")]
        public Specialty Specialty { get; set; }
        [ForeignKey("GraduateId")]
        public Graduate Graduate { get; set; }
    }
}