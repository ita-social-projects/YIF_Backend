using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class UniversityPostApiModel
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
        public ImageApiModel ImageApiModel { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string UniversityAdminEmail { get; set; }
    }
}
