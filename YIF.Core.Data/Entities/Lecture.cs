using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    //Where we use this table?
    public class Lecture
    {
        [Key, ForeignKey("User")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string InstitutionOfEducationId { get; set; }

        [ForeignKey("InstitutionOfEducationId")]
        public virtual InstitutionOfEducation InstitutionOfEducation { get; set; }
        /// <summary>
        /// Link to Identity user
        /// </summary>
        public virtual DbUser User { get; set; }
    }
}
