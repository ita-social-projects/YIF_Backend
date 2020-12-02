using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class SchoolAdmin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string SchoolId { get; set; }

        [ForeignKey("SchoolId")]
        public School School { get; set; }
        /// <summary>
        /// Link to school moderator
        /// </summary>
        //public SchoolModerator Moderator { get; set; }
        /// <summary>
        /// Link to Identity user
        /// </summary>
        [ForeignKey("Id")]
        public DbUser User { get; set; }
    }
}
