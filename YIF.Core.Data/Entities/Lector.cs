using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;
using System.Collections.Generic;

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

        [ForeignKey("User")]
        public string UserId { get; set; }
        public DbUser User { get; set; }

        public string DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public string SpecialtyId { get; set; }
        [ForeignKey("SpecialtyId")]
        public Specialty Specialty { get; set; }

        public ICollection<Discipline> Disciplines { get; set; }
    }
}
