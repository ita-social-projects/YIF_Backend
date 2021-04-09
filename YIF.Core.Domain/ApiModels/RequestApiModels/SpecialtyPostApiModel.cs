using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class SpecialtyPostApiModel
    {
        public string Name { get; set; }
        public string DirectionId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
