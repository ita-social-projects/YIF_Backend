using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class SpecialityPostApiModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DirectionId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
