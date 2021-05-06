using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class Lecture : BaseEntity
    {
        public string InstitutionOfEducationId { get; set; }

        [ForeignKey("InstitutionOfEducationId")]
        public InstitutionOfEducation InstitutionOfEducation { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public DbUser User { get; set; }
    }
}
