using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class InstitutionOfEducationAdminApiModel
    {
        /// <summary>
        /// Institution of education Id
        /// </summary>
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        [Required]
        [StringLength(255)]
        public string InstitutionOfEducationId { get; set; }

        /// <summary>
        /// Administrator email
        /// </summary>
        /// <example>admin@kpi.ua</example>
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string AdminEmail { get; set; }
    }
}
