﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class UniversityAdmin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UniversityId { get; set; }

        [ForeignKey("UniversityId")]
        public virtual University University { get; set; }
        /// <summary>
        /// Link to university moderator
        /// </summary>
        public virtual UniversityModerator Moderator { get; set; }     
    }
}
