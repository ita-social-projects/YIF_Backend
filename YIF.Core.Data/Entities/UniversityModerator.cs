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
        
        [ForeignKey("AdminId")]
        public string AdminId { get; set; }
        public virtual UniversityAdmin Admin { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual DbUser User { get; set; }

    }
}
