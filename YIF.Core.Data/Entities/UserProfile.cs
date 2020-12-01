using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YIF.Core.Data.Entities
{
    public class UserProfile
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; }

        /// <summary>
        /// Ім'я користувача
        /// </summary>
        [Required, StringLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Прізвище користувача
        /// </summary>
        [Required, StringLength(255)]
        public string Surname { get; set; }

        /// <summary>
        /// Фото користувача
        /// </summary>
        [StringLength(255)]
        public string Photo { get; set; }

        /// <summary>
        /// Дата народження користувача
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Номер телефона користувача
        /// </summary>
        [StringLength(20)]
        public string Phone { get; set; }

        /// <summary>
        /// Дата реєстрації
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        public virtual DbUser User { get; set; }
    }
}
