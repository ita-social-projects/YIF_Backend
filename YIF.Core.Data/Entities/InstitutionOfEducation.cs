﻿using System;
using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public enum InstitutionOfEducationType
    {
        University,
        College
    }
    public class InstitutionOfEducation : BaseEntity
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Site { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public InstitutionOfEducationType InstitutionOfEducationType { get; set; }
        public DateTime StartOfCampaign { get; set; }
        public DateTime EndOfCampaign { get; set; }

        /// <summary>
        /// List of institutionOfEducation admins
        /// </summary>
        public virtual ICollection<InstitutionOfEducationAdmin> Admins { get; set; }
        /// <summary>
        /// List of institutionOfEducation lectures
        /// </summary>
        public virtual ICollection<Lecture> Lectures { get; set; }
        /// <summary>
        /// List of graduates who liked the institutionOfEducation
        /// </summary>
        public virtual ICollection<InstitutionOfEducationToGraduate> InstitutionOfEducationGraduates { get; set; }
        public virtual ICollection<SpecialtyToInstitutionOfEducationToGraduate> SpecialtyToInstitutionOfEducationToGraduates { get; set; }

    }
}