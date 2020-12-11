using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class SchoolModerator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string SchoolId { get; set; }
        public string AdminId { get; set; }

        [ForeignKey("SchoolId")]
        public School School { get; set; }
        [ForeignKey("AdminId")]
        public SchoolAdmin Admin { get; set; }
        /// <summary>
        /// Link to Identity user
        /// </summary>      
        public DbUser User { get; set; }
    }
}
