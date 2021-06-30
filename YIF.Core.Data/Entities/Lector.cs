using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class Lector : BaseEntity
    {
        public string InstitutionOfEducationId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string ImagePath { get; set; }
        public string Description { get; set; }

        [ForeignKey("InstitutionOfEducationId")]
        public InstitutionOfEducation InstitutionOfEducation { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public DbUser User { get; set; }

        public string DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
    }
}
