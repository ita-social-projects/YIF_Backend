using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class UniversityAdmin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public bool IsBanned { get; set; } = false;

        [ForeignKey("UniversityId")]
        public string UniversityId { get; set; }
        public virtual University University { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual DbUser User { get; set; }

        public virtual ICollection<UniversityModerator> Moderators { get; set; }
    }
}
