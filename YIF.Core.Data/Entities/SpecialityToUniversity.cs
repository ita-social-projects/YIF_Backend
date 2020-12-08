﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YIF.Core.Data.Entities
{
    public class SpecialityToUniversity : BaseEntity
    {
        public string SpecialityId { get; set; }
        public string UniversityId { get; set; }

        [ForeignKey("SpecialityId")]
        public Speciality Speciality { get; set; }
        [ForeignKey("UniversityId")]
        public University University { get; set; }
    }
}
