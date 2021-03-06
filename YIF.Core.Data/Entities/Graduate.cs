﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class Graduate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string SchoolId { get; set; }

        [ForeignKey("SchoolId")]
        public virtual School School { get; set; }
        /// <summary>
        /// Link to Identity user
        /// </summary>
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual DbUser User { get; set; }

        /// <summary>
        /// List of favorite universities
        /// </summary>
        public virtual ICollection<UniversityToGraduate> UniversityGraduates { get; set; }
        public virtual ICollection<SpecialtyToUniversityToGraduate> SpecialtyToUniversityToGraduates { get; set; }

    }
}
