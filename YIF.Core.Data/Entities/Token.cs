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
        public virtual DbUser User { get; set; }

        /// <summary>
        /// Get or set refresh token
        /// </summary>
        [Required, StringLength(100)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Get or set refresh token expiry time
        /// </summary>
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
