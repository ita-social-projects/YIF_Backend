using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class SchoolModerator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string SchoolId { get; set; } //not need
        public string AdminId { get; set; }

        [ForeignKey("SchoolId")]
        public virtual School School { get; set; } //not need
        [ForeignKey("AdminId")]
        public virtual SchoolAdmin Admin { get; set; }
        /// <summary>
        /// Link to Identity user
        /// </summary>
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual DbUser User { get; set; }
    }
}
