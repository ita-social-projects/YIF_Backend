using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class Graduate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string SchoolId { get; set; }

        [ForeignKey("SchoolId")]
        public School School { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public DbUser User { get; set; }

        public ICollection<InstitutionOfEducationToGraduate> InstitutionOfEducationGraduates { get; set; }
        public ICollection<SpecialtyToInstitutionOfEducationToGraduate> SpecialtyToInstitutionOfEducationToGraduates { get; set; }
        public ICollection<SpecialtyToGraduate> SpecialtyToGraduates { get; set; }

    }
}
