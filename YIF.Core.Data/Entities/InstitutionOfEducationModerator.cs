using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class InstitutionOfEducationModerator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        
        [ForeignKey("AdminId")]
        public string AdminId { get; set; }
        public InstitutionOfEducationAdmin Admin { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public DbUser User { get; set; }

    }
}
