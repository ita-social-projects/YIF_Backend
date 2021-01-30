using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Entities
{
    [Table("tblUserProfiles")]
    public class UserProfile
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [StringLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// User surname
        /// </summary>
        [StringLength(255)]
        public string Surname { get; set; }

        /// <summary>
        /// User middle name
        /// </summary>
        [StringLength(255)]
        public string MiddleName { get; set; }

        /// <summary>
        /// User photo
        /// </summary>
        [StringLength(255)]
        public string Photo { get; set; }

        /// <summary>
        /// User date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Registration date
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        public virtual DbUser User { get; set; }
    }
}
