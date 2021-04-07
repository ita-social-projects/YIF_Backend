using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    public class SuperAdmin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public DbUser User { get; set; }
    }
}
