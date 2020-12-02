using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class UniversityModerator
    {
        [Key, ForeignKey("User")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UniversityId { get; set; }
        public string AdminId { get; set; }

        [ForeignKey("AdminId")]
        public UniversityAdmin Admin { get; set; }
        [ForeignKey("UniversityId")]
        public University University { get; set; }
        /// <summary>
        /// Link to Identity user
        /// </summary>
        public DbUser User { get; set; }
    }
}
