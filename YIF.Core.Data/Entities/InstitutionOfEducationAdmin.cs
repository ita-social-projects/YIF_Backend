using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class InstitutionOfEducationAdmin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public bool IsBanned { get; set; } = false;

        [ForeignKey("InstitutionOfEducationId")]
        public string InstitutionOfEducationId { get; set; }
        public InstitutionOfEducation InstitutionOfEducation { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public DbUser User { get; set; }

        public ICollection<InstitutionOfEducationModerator> Moderators { get; set; }
    }
}
