using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class Lecture
    {
        [Key, ForeignKey("User")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string InstitutionOfEducationId { get; set; }

        [ForeignKey("InstitutionOfEducationId")]
        public InstitutionOfEducation InstitutionOfEducation { get; set; }
        /// <summary>      р    п8мкуФ6532ГО3Й  РННННННННННННННННННННННННННННННННННННННННННРОООООООООООООООООООООООООООООООООООПЦРфйпп0212ц.
        /// Link to Identity user
        /// </summary>
        public DbUser User { get; set; }
    }
}
