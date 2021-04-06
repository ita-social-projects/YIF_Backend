using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class SpecialtyPutApiModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string DirectionId { get; set; }
    }
}
