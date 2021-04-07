using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    [Table("tblTokens")]
    public class Token
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; }
        public DbUser User { get; set; }

        [Required, StringLength(100)]
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
