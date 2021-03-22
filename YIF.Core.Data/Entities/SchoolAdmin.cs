using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class SchoolAdmin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string SchoolId { get; set; }

        [ForeignKey("SchoolId")]
        public virtual School School { get; set; }
        /// <summary>
        /// Link to school moderator
        /// </summary>
        public virtual ICollection<SchoolModerator> SchoolModerators { get; set; }
    }
}
