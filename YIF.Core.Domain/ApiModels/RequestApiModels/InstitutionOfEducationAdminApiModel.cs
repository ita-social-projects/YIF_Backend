using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class InstitutionOfEducationAdminApiModel
    {
        [Required]
        [StringLength(255)]
        public string InstitutionOfEducationId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string AdminEmail { get; set; }
    }
}
