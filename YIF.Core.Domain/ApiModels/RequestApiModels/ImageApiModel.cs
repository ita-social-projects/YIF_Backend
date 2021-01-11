using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class ImageApiModel
    {
        /// <summary>
        /// Base64 image
        /// </summary>
        [Required]
        public string Photo { get; set; }
    }
}
