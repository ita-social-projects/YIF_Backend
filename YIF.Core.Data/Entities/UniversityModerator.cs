using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class UniversityModerator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UniversityId { get; set; }
        public string AdminId { get; set; }

        [ForeignKey("AdminId")]
        public virtual UniversityAdmin Admin { get; set; }
        [ForeignKey("UniversityId")]
        public virtual University University { get; set; }
        /// <summary>
        /// Link to Identity user
        /// </summary>
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual DbUser User { get; set; }
    }
}
