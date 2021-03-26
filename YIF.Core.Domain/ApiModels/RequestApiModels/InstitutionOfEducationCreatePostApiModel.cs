using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class InstitutionOfEducationCreatePostApiModel : InstitutionOfEducationPostApiModel
    {
        [Required]
        public override ImageApiModel ImageApiModel { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string InstitutionOfEducationAdminEmail { get; set; }
    }
}
