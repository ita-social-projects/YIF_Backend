using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class InstitutionOfEducationPostApiModel
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Abbreviation { get; set; }

        [Required]
        [Url]
        [StringLength(255)]
        public string Site { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public float Lat { get; set; }

        [Required]
        public float Lon { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public InstitutionOfEducationType InstitutionOfEducationType { get; set; }

        public virtual ImageApiModel ImageApiModel { get; set; }
    }
}
