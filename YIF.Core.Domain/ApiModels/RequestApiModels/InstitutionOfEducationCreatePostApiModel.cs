using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class InstitutionOfEducationCreatePostApiModel : InstitutionOfEducationPostApiModel
    {
        /// <summary>
        /// Image of institution of education
        /// </summary>
        [Required]
        public override ImageApiModel ImageApiModel { get; set; }

        /// <summary>
        /// Institution ef education admin`s email
        /// </summary>
        /// <example>admin@kpi.ua</example>
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string InstitutionOfEducationAdminEmail { get; set; }
    }
}
